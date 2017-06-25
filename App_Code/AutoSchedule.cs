using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AutoSchedule
/// </summary>
public class AutoSchedule
{
    private int N, M;//班级数//周课时数//二维表规模
    private Random GetRandnum = new Random();

    private Struct_CourseName[] courseList;//学科序号 ---> 学时
    private TeaInfo[,] teacherArrange;//班级序号,学科序号 ---> 学科号,老师号，。。。
    //学科号对应的周课时数//语文 ---> 6
    private Dictionary<string, int> course_week_sum = new Dictionary<string, int>();

    public Struct_CourseName[] CourseList
    {
        get
        {
            return courseList;
        }
        set
        {
            courseList = value;
        }
    }
    public TeaInfo[,] TeacherArrange
    {
        get
        {
            return teacherArrange;
        }
        set
        {
            teacherArrange = value;
        }
    }

    public AutoSchedule(int _numOfCourse)
    {
        courseList = new Struct_CourseName[_numOfCourse];
    }
    public AutoSchedule(int _numOfClass, int _sumOfLesson)
    {
        teacherArrange = new TeaInfo[_numOfClass, _sumOfLesson];
    }


    private struct ch
    {
        //public string[,] t_id;//编码
        public Tea[,] teacher;
        public int fit;//课表总适合度 
        public int[] fitstu;//班级对应的学生课表fit值
        public double d;//比例 
        public int r;//轮盘赌次数
        public int rs;//赌盘因子 
    };
    private ch[] c;//种群
    ch ans;//当前搜索到最好的解

    private int S;//初始种群规模 //2的倍数
    private int X;//选择操作之后的规模//保证为2的倍数

    private int fitsum = 0;//当前一代fit总值
    private const double Pm = 0.01, Pc = 0.7;//变异和交叉概率


    public AutoSchedule()
    {
    }


    private void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    //t班的学生fit值更新//init()以及班级课表变动的时候触发
    private void fresh_fitstu(ref ch cc, int t)
    {
        int fitvalue = 100;//BESTFITNESS
        Dictionary<string, int> max_map = new Dictionary<string, int>();
        max_map.Clear();
        Dictionary<string, int> min_map = new Dictionary<string, int>();
        min_map.Clear();

        string yuwencno = teacherArrange[0, 0].Courseno;
        for (int i = 1; i <= 5; i++)//每个星期5天课程确定
        {
            //对于每天课程进行评判
            Dictionary<string, int> c_map = new Dictionary<string, int>();
            c_map.Clear();

            //c_map更新//t班星期i的老师工号对应课时数
            for (int j = 1; j <= M / 5; j++)
            {
                if (cc.teacher[t, (i - 1) * M / 5 + j].cno.Equals(yuwencno))
                {
                    ////////////////扣分明细///////////////
                    //语文课尽量靠前
                    fitvalue -= (j - 1) * 3;
                }
                if (c_map.ContainsKey(cc.teacher[t, (i - 1) * M / 5 + j].id))
                {
                    c_map[cc.teacher[t, (i - 1) * M / 5 + j].id]++;
                }
                else
                {
                    c_map.Add(cc.teacher[t, (i - 1) * M / 5 + j].id, 1);
                }
            }

            //判断每天课时评分
            foreach (var item in c_map)
            {
                //学生同一天同一门课程
                if (item.Value <= 1) continue;
                else if (item.Value == 2)
                {
                    //第一次和第二次上课时间
                    int early = 0, late = 0;
                    for (int j = 1; j <= M / 5; j++)
                    {
                        if (item.Equals(cc.teacher[t, (i - 1) * M / 5 + j].id))
                        {
                            if (early == 0)
                            {
                                early = j;
                            }
                            else
                            {
                                late = j;
                                break;
                            }
                        }
                    }

                    ////////////////扣分明细///////////////
                    //学生一天两节的同一门课间距最小//或分上下午
                    if ((early > 4 && late > 4) || (early < 5 && late < 5))
                    {
                        if (late - early - 1 == 0)
                        {
                            //相邻可以接受
                        }
                        else
                        {
                            //同上午同下午不相邻扣分
                            fitvalue -= (late - early) * 10;
                        }

                    }
                    else
                    {
                        ;//可以接受上下午两节
                    }
                }
                else//>=3
                {
                    ////////////////扣分明细///////////////
                    //学生一天同一门课程不得超过2节
                    fitvalue -= item.Value * 10;//item.Value课程的课时数
                }
            }

            //更新min_map&&max_map
            foreach (var item in c_map)
            {
                if (min_map.ContainsKey(item.Key))
                {
                    min_map[item.Key] = min_map[item.Key] < item.Value ? min_map[item.Key] : item.Value;
                    max_map[item.Key] = max_map[item.Key] > item.Value ? max_map[item.Key] : item.Value;
                }
                else
                {
                    min_map.Add(item.Key, item.Value);
                    max_map.Add(item.Key, item.Value);
                }
            }
        }

        foreach (var item in max_map)
        {
            if (item.Value - min_map[item.Key] > 1)
            {
                ////////////////扣分明细///////////////
                //学生一周的用一门课时数的极差应不大于1//例如语文5天课时数分别为12111
                fitvalue -= (item.Value - min_map[item.Key]);
            }
        }

        if (fitvalue < 0) fitvalue = 0;//非负数
        cc.fitstu[t] = fitvalue;//每个班的fit_stu
    }

    //整个课表的学生fit值
    private int fit_stu(ref ch cc)
    {
        int sum = 0;
        //N个班级之和
        for (int t = 1; t < N; t++)
        {
            sum += cc.fitstu[t];//.fitstu[]从init()初始化后从头到尾凡有变动即更新
        }
        return sum;
    }

    //整个课表老师的fit值
    private int fit_tch(ref ch cc)
    {
        int fitvalue = 100;
        Dictionary<string, int> max_map = new Dictionary<string, int>();
        max_map.Clear();
        Dictionary<string, int> min_map = new Dictionary<string, int>();
        min_map.Clear();

        Dictionary<string, int> t_map = new Dictionary<string, int>();
        for (int t = 1; t <= 5; t++)//星期t
        {
            t_map.Clear();
            for (int i = 1; i <= N; i++)//i班
            {
                for (int j = 1; j <= M / 5; j++)//第j节课
                {
                    if (t_map.ContainsKey(cc.teacher[i, (t - 1) * M / 5 + j].id))
                    {
                        t_map[cc.teacher[i, (t - 1) * M / 5 + j].id]++;
                    }
                    else
                    {
                        t_map.Add(cc.teacher[i, (t - 1) * M / 5 + j].id, 1);
                    }
                }
            }


            //更新min_map&&max_map
            foreach (var item in t_map)
            {
                if (min_map.ContainsKey(item.Key))
                {
                    min_map[item.Key] = min_map[item.Key] < item.Value ? min_map[item.Key] : item.Value;
                    max_map[item.Key] = max_map[item.Key] > item.Value ? max_map[item.Key] : item.Value;
                }
                else
                {
                    min_map.Add(item.Key, item.Value);
                    max_map.Add(item.Key, item.Value);
                }

                ////////////////扣分明细///////////////
                //老师一天上课不超过4节
                if (item.Value > 4)//老师一天超过4节课
                {
                    //fitvalue -= item.Value;
                }
            }
        }

        foreach (var item in max_map)
        {
            if (item.Value - min_map[item.Key] > 1)
            {
                fitvalue -= (item.Value - min_map[item.Key]);
            }
        }

        return fitvalue;
    }

    //计算一个课表的总fit值
    private int count_fit(ref ch cc)
    {
        // 课表总fit值为 每个班级fit_stu + 每个老师fit_tch
        int fitvaluesum = fit_stu(ref cc) + fit_tch(ref cc);

        //让个体适应度大于0
        if (fitvaluesum < 0)
        {
            fitvaluesum = 0;
        }
        return cc.fit = fitvaluesum;
    }

    //返回某个冲突的行数//0代表无冲突
    private int Conflict(ref ch cc, int i, int j, string str)
    {
        if (str.Equals("nullstring")) return 0;//空字符串代表初始化时候未放入
        for (int t = 1; t <= N; t++)
        {
            if (t == i) continue;
            if (cc.teacher[t, j].id == str)
            {
                return t;//true
            }
        }
        return 0;//false
    }

    //在可行解(某染色体)cc中,t,i与t,j交换无冲突
    private bool SwapConflict(ref ch cc, int t, int i, int j)
    {
        return Conflict(ref cc, t, i, cc.teacher[t, j].id) > 0 || Conflict(ref cc, t, j, cc.teacher[t, i].id) > 0;
    }

    private void RemoveConflict(ref ch cc, int i, int j)
    {
        //没有冲突解决个毛线
        if (Conflict(ref cc, i, j, cc.teacher[i, j].id) == 0 || i == 0)
        {
            return;
        }

        int pos = 0;//待交换的地方
        //课时大于等于5//语数外
        if (course_week_sum[cc.teacher[i, j].cno] >= 5)
        {
            int day, tim;//星期几//节次//
            day = (j - 1) / 8 + 1;
            tim = (j - 1) % 8 + 1;
            //j = (day-1)*8+tim

            int count = 0;
            for (int t = 1; t <= 8; t++)
            {
                if (cc.teacher[i, (day - 1) * 8 + t].id == cc.teacher[i, j].id)
                {
                    count++;
                }
            }

            //以下均不考虑死循环

            //这一天只有一节课
            if (count == 1)
            {
                //和当天课程交换
                pos = (day - 1) * 8 + GetRandnum.Next(1, 9);//1 ~ 8
                while (pos % 8 == tim || SwapConflict(ref cc, i, pos, j))
                {
                    pos = (day - 1) * 8 + GetRandnum.Next(1, 9);
                }
            }
            else//两节课以上
            {
                //和任意非5周课时以上课程交换
                pos = GetRandnum.Next(1, M + 1);//1 ~ M

                //if ((pos - 1) / 8 + 1 == day) //和当天交换
                {
                    pos = (day - 1) * 8 + GetRandnum.Next(1, 9);//1 ~ 8
                    while (pos % 8 == tim || SwapConflict(ref cc, i, pos, j))
                    {
                        pos = (day - 1) * 8 + GetRandnum.Next(1, 9);
                    }
                }
                /*else//其他天
                {
                    pos = GetRandnum.Next(1, M + 1);
                    while (course_week_sum[cc.teacher[i, pos].cno] >= 5 || (pos - 1) / 8 + 1 == day || Conflict(ref cc, i, pos, cc.teacher[i, j].id) > 0 || Conflict(ref cc, i, j, cc.teacher[i, pos].id) > 0)
                    {
                        pos = GetRandnum.Next(1, M + 1);
                    }
                }*/
            }
        }
        else//课时数小于5//美术音乐之类
        {
            pos = GetRandnum.Next(1, M + 1);//1 ~ M
            while (course_week_sum[cc.teacher[i, pos].cno] >= 5 || pos == j || SwapConflict(ref cc, i, pos, j))
            {
                pos = GetRandnum.Next(1, M + 1);
            }
        }

        Swap<Tea>(ref cc.teacher[i, j], ref cc.teacher[i, pos]);
    }

    private void RemoveConflict1(ref ch cc, int i, int j)
    {
        //没有冲突解决个毛线
        if (Conflict(ref cc, i, j, cc.teacher[i, j].id) == 0 || i == 0)
        {
            return;
        }

        int t = GetRandnum.Next(1, M + 1);

        int conj = 0, cont = 0;//与 i行j,t列 发生冲突的行数
        int loop = 0;
        int maxloop = M;
        while (loop++ < maxloop)//防止死循环
        {
            //防止自己和自己调换
            while (t == j) t = GetRandnum.Next(1, M + 1);

            //与 i行j,t列 发生冲突的行数
            conj = Conflict(ref cc, i, j, cc.teacher[i, t].id);
            cont = Conflict(ref cc, i, t, cc.teacher[i, j].id);

            //有冲突继续随机选择//希望找到一个位置交换课程而消除冲突
            if (conj > 0 || cont > 0) continue;//尽量避免调换之后还有冲突
            else break;
        }

        Swap<Tea>(ref cc.teacher[i, j], ref cc.teacher[i, t]);

        //有可能还有冲突//递归消除冲突
        //改动其他行
        //if (conj > 0) { RemoveConflict(ref cc, conj, j); }
        //if (cont > 0) { RemoveConflict(ref cc, cont, t); }

        //不改动其他行
        if (conj > 0) { RemoveConflict(ref cc, i, j); }
        if (cont > 0) { RemoveConflict(ref cc, i, t); }
    }

    private void CreateIndividual(ref ch cc)
    {
        //对空染色体cc进行初始化为一个可行解
        //对每个空位塞课程，同时消除冲突
        for (int i = 1; i <= N; i++)//班级
        {
            for (int j = 0; j < courseList.Length; j++)//库里的课程数据
            {
                //语数外
                if (courseList[j].Hour1 >= 5)
                {
                    //先5,10,15课时分别安排不同5天
                    for (int count = courseList[j].Hour1 / 5; count >= 1; count--)
                    {
                        for (int t = 1; t <= 5; t++)
                        {
                            int pos = (t - 1) * 8 + GetRandnum.Next(1, 9);//pos刚好在星期t
                            while (!cc.teacher[i, pos].id.Equals("nullstring"))
                            {
                                pos = (t - 1) * 8 + GetRandnum.Next(1, 9);//pos刚好在星期t
                            }
                            //塞到这一天的空位然后消除冲突
                            cc.teacher[i, pos].id = teacherArrange[i - 1, j].No;
                            cc.teacher[i, pos].cno = teacherArrange[i - 1, j].Courseno;
                            //然后消除矛盾
                            RemoveConflict(ref cc, i, pos);
                        }
                    }

                    //剩下课时不同天
                    Random_Permutation RP = new Random_Permutation(5);
                    int[] randpermutation = RP.NextRandom();

                    for (int t = 0; t < courseList[j].Hour1 % 5; t++)
                    {
                        int pos = 8 * (randpermutation[t]) + GetRandnum.Next(1, 9);
                        while (!cc.teacher[i, pos].id.Equals("nullstring"))
                        {
                            pos = 8 * (randpermutation[t]) + GetRandnum.Next(1, 9);
                        }
                        //塞到这一天的空位然后消除冲突
                        cc.teacher[i, pos].id = teacherArrange[i - 1, j].No;
                        cc.teacher[i, pos].cno = teacherArrange[i - 1, j].Courseno;
                        //然后消除矛盾
                        RemoveConflict(ref cc, i, pos);
                    }
                }
                else//政史地
                {
                    for (int t = 1; t <= courseList[j].Hour1; t++)//按课时数量塞
                    {

                        int k = GetRandnum.Next(1, M + 1);//随机塞的位置为i，k
                        while (!cc.teacher[i, k].id.Equals("nullstring"))//不为空就继续随机，直到随机到一个空位
                        {
                            k = GetRandnum.Next(1, M + 1);
                        }
                        cc.teacher[i, k].id = teacherArrange[i - 1, j].No;//塞进表
                        cc.teacher[i, k].cno = teacherArrange[i - 1, j].Courseno;
                        RemoveConflict(ref cc, i, k);//同时消除冲突
                    }
                }

            }
        }

        for (int t = 1; t <= N; t++)
        {
            fresh_fitstu(ref cc, t);//init()更新fit值,每次班级课表变动也要更新
        }
    }

    private void CreateIndividual1(ref ch cc)
    {
        //对空染色体cc进行初始化为一个可行解
        //对每个空位塞课程，同时消除冲突
        for (int i = 1; i <= N; i++)//班级
        {
            for (int j = 0; j < courseList.Length; j++)//库里的课程数据
            {
                for (int t = 1; t <= courseList[j].Hour1; t++)//按课时数量塞
                {
                    int k = GetRandnum.Next(1, M + 1);//随机塞的位置为i，k
                    while (!cc.teacher[i, k].id.Equals("nullstring"))//不为空就继续随机，直到随机到一个空位
                    {
                        k = GetRandnum.Next(1, M + 1);
                    }
                    cc.teacher[i, k].id = teacherArrange[i - 1, j].No;//塞进表
                    cc.teacher[i, k].cno = teacherArrange[i - 1, j].Courseno;
                    RemoveConflict(ref cc, i, k);//同时消除冲突
                }
            }
        }

        for (int t = 1; t <= N; t++)
        {
            fresh_fitstu(ref cc, t);//init()更新fit值,每次班级课表变动也要更新
        }
    }


    private void init()
    {
        //N,M初始化
        N = teacherArrange.GetLength(0);
        M = 0;
        for (int t = 0; t < courseList.Length; t++)
        {
            M += courseList[t].Hour1;
            course_week_sum.Add(teacherArrange[0, t].Courseno, courseList[t].Hour1);
        }

        //规模为基因数目2倍
        S = N * 2;
        //选择操作之后种群的个数
        X = ((int)((double)S * Pc)) / 2 * 2;//保证为正偶数

        //染色体种群初始化
        c = new ch[S + 2];//用到c[S+1]
        for (int t = 1; t <= S; t++)
        {
            c[t].teacher = new Tea[N + 1, M + 1];
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= M; j++)
                {
                    c[t].teacher[i, j] = new Tea();
                    c[t].teacher[i, j].id = "nullstring";
                    c[t].teacher[i, j].cno = "nullstring";
                }
            }
            c[t].fitstu = new int[N + 1];
        }
        ans.teacher = new Tea[N + 1, M + 1];
        ans.fit = -1;//设置一个小值

        course_week_sum.Add("nullstring", 0);//防止后面调用产生空指针

        for (int t = 1; t <= S; t++)
        {
            CreateIndividual(ref c[t]);
        }

        fitsum = 0;
        for (int t = 1; t <= S; t++)
        {
            if (ans.fit < c[t].fit)
            {
                ans = c[t];
            }
            fitsum += c[t].fit;
        }

        for (int t = 1; t <= S; t++)
        {
            c[t].d = (double)c[t].fit / (double)fitsum;
        }
    }

    //轮盘赌
    private void Roulette()
    {
        //Random R = new Random ();
        c[1].rs = 1;
        for (int t = 2; t <= S; t++)
        {
            c[t].rs = c[t - 1].rs + c[t - 1].fit;
        }
        c[S + 1].rs = c[S].rs + c[S].fit;

        for (int t = 1; t <= S; t++)
        {
            c[t].r = 0;
        }

        //从S个个体中选择X个个体(S-X为正偶数)
        for (int t = 1; t <= X; t++)
        {
            //轮盘赌判断randnum落在哪个个体上
            int randnum = GetRandnum.Next(0, fitsum + 1);
            int k = 1;
            while (randnum >= c[k + 1].rs)
            {
                k++;
            }
            c[k].r++;
        }
    }

    //挑选 
    private void Select()
    {
        Roulette();//.r

        //选择了X个个体
        ch[] delta = new ch[X + 1];

        int k = 1;
        //从S个个体中选择
        for (int i = 1; i <= S; i++)
        {
            //将c[i].r个i个体选择出来
            for (int j = 1; j <= c[i].r; j++)
            {
                delta[k] = c[i];//选择过程
                k++;
            }
        }

        //选择出来后为X个个体
        for (int t = 1; t <= X; t++)
        {
            c[t] = delta[t];
        }
    }

    //交叉细节 
    private void exchange(int t1, int t2)
    {
        //基因位置//交换的班级位置
        int pos = GetRandnum.Next(1, N + 1);

        for (int t = 1; t <= M; t++)
        {
            Swap<Tea>(ref c[t1].teacher[pos, t], ref c[t2].teacher[pos, t]);
        }

        for (int t = 1; t <= M; t++)
        {
            RemoveConflict(ref c[t1], pos, t);
            RemoveConflict(ref c[t2], pos, t);
        }

        //每次修改班课表均更新
        fresh_fitstu(ref c[t1], pos);
        fresh_fitstu(ref c[t2], pos);
    }

    //交叉 
    private void Crossover()
    {
        int k = (S - X) / 2;//从X规模刚好繁殖到S的交配次数
        for (int t = 1; t <= k; t++)
        {
            //t1,t2个体交叉
            int t1 = GetRandnum.Next(1, S + 1);
            int t2 = GetRandnum.Next(1, S + 1);
            while (t1 == t2) t2 = GetRandnum.Next(1, S + 1);

            //将t1,t2先加入种群
            c[X + k * 2 - 1] = c[t1];
            c[X + k * 2] = c[t2];
            //交叉后的新的2个个体代替t1,t2个体
            exchange(t1, t2);
        }
    }

    //变异 
    private void Mutation()
    {
        if ((double)GetRandnum.Next(0, 5000000) / 5000000.0 <= Pm)
        {
            int nk, posk;
            //随机个体
            nk = GetRandnum.Next(1, S + 1);//个体号
            //随机班级
            posk = GetRandnum.Next(1, N + 1);//哪一位基因班级

            //随机两节课
            int t1 = GetRandnum.Next(1, M + 1);
            int t2 = GetRandnum.Next(1, M + 1);

            //t1,t2位置均不为周课时大于或等于5的
            while (t1 == t2 || course_week_sum[c[nk].teacher[posk, t1].cno] >= 5 || course_week_sum[c[nk].teacher[posk, t2].cno] >= 5)
            {
                t1 = GetRandnum.Next(1, M + 1);
                t2 = GetRandnum.Next(1, M + 1);
            }

            //第nk个 个体 ， 第posk个 班级 ，第 t1,t2 课时对调
            Swap<Tea>(ref c[nk].teacher[posk, t1], ref c[nk].teacher[posk, t2]);

            RemoveConflict(ref c[nk], posk, t1);
            RemoveConflict(ref c[nk], posk, t2);

            //更新
            fresh_fitstu(ref c[nk], posk);
        }
    }

    //遗传算法步骤 
    private void Generating()
    {
        Select();//选择 
        Crossover();//交叉 
        Mutation();// 变异 
    }

    //更新下一代基本成员值
    private void Estimate()
    {
        //当前一代的所有个体fit值之和
        fitsum = 0;
        for (int t = 1; t <= S; t++)
        {
            //更新每个个体解的fit值
            c[t].fit = count_fit(ref c[t]);
            //更新当前最佳解
            if (ans.fit < c[t].fit)
            {
                ans = c[t];
            }
            //总fit值
            fitsum += c[t].fit;
        }
        //更新其他辅助值
        for (int t = 1; t <= N; t++)
        {
            c[t].d = (double)c[t].fit / (double)fitsum;
            c[t].r = 0;
        }
    }

    //手工调t班的课表
    private void ChangeSchedule(int t)
    {
        //语文课一定在上午
        //英语课尽量靠前
        //一天的两节主课连堂
        //副科一天一节
        string yuwentch, engtch;//老师id
        yuwentch = teacherArrange[t - 1, 0].No;
        engtch = teacherArrange[t - 1, 2].No;

        for (int i = 1; i <= 5; i++)//星期几
        {
            Dictionary<string, int> course_sum = new Dictionary<string, int>();
            course_sum.Clear();

            for (int j = 1; j <= 8; j++)//这一天第几节课
            {
                int g = (i - 1) * 8 + j;//在1~40哪一位

                if (course_sum.ContainsKey(ans.teacher[t, g].id))
                {
                    course_sum[ans.teacher[t, g].id]++;
                }
                else
                {
                    course_sum.Add(ans.teacher[t, g].id, 1);
                }
            }

            //语文提前
            if (course_sum[yuwentch] == 1)
            {
                for (int j = 1; j <= 8; j++)
                {
                    int g = (i - 1) * 8 + j;//在1~40哪一位
                    if (ans.teacher[t, g].id == yuwentch)
                    {
                        for (int delta = 1; delta <= 8; delta++)
                        {
                            int delta_g = (i - 1) * 8 + delta;
                            //自己跟自己交换意味着不能提前了
                            if (!SwapConflict(ref ans, t, g, delta_g))
                            {
                                Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, delta_g]);//提前 
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else//course_sum[yuwentch] == 2//默认周课时12以内
            {
                //定位这两节课
                int g1 = 0, g2 = 0;
                for (int j = 1; j <= 8; j++)
                {
                    int g = (i - 1) * 8 + j;//在1~40哪一位
                    if (ans.teacher[t, g].id == yuwentch)
                    {
                        if (g1 == 0) g1 = g;
                        else
                        {
                            g2 = g;
                            break;
                        }
                    }
                }

                //放在上午的3种连堂情况
                bool findsolution = false;//是否有这样的连堂情况
                for (int j = 1; j <= 3; j++)
                {
                    int g = (i - 1) * 8 + j;//在1~40哪一位
                    if (!SwapConflict(ref ans, t, g, g1) && !SwapConflict(ref ans, t, g + 1, g2))
                    {
                        findsolution = true;
                        Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, g1]);
                        Swap<Tea>(ref ans.teacher[t, g + 1], ref ans.teacher[t, g2]);
                        break;
                    }
                    else if (!SwapConflict(ref ans, t, g, g2) && !SwapConflict(ref ans, t, g + 1, g1))
                    {
                        findsolution = true;
                        Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, g2]);
                        Swap<Tea>(ref ans.teacher[t, g + 1], ref ans.teacher[t, g1]);
                        break;
                    }
                }

                //没有这样的连堂情况//g1提前，g2换到另一天
                if (!findsolution)
                {
                    //g1提前
                    for (int j = 1; j <= 8; j++)
                    {
                        int g = (i - 1) * 8 + j;//在1~40哪一位

                        //自己跟自己交换意味着不能提前了
                        if (!SwapConflict(ref ans, t, g, g1))
                        {
                            Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, g1]);//提前 
                        }
                    }

                    //g2换一天
                    //之前//优化了
                    for (int j = 1; j < i; j++)//星期
                    {
                        int count = 0;
                        for (int delta = 1; delta <= 8; delta++)
                        {
                            int g = (j - 1) * 8 + delta;
                            if (ans.teacher[t, g].id == ans.teacher[t, g2].id)
                            {
                                count++;
                            }
                        }
                        if (count >= 2)
                        {
                            //已经有两节课以上
                            continue;
                        }

                        //插到上面
                        for (int delta = 1; delta < 8; delta++)
                        {
                            int g = (j - 1) * 8 + delta;

                            //找到科目地方
                            if (ans.teacher[t, g + 1].id == ans.teacher[t, g2].id)
                            {
                                //和副科交换
                                if (course_week_sum[ans.teacher[t, g].cno] < 5 && !SwapConflict(ref ans, t, g, g2))
                                {
                                    //成功交换
                                    Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, g2]);

                                    if (course_sum.ContainsKey(ans.teacher[t, g].id))
                                    {
                                        course_sum[ans.teacher[t, g].id]++;
                                    }
                                    else
                                    {
                                        course_sum.Add(ans.teacher[t, g].id, 1);
                                    }

                                    //语文换到另一天
                                    //course_sum[ans.teacher[t,g2].id] --;//ans.teacher[t,g2].id=yuwentch 

                                    findsolution = true;
                                }
                                else
                                {
                                    //不是副科//下一个循环继续找
                                    break;
                                }

                            }
                        }

                        //插到上面不行没解决//插到下方
                        if (!findsolution)
                        {
                            for (int delta = 2; delta <= 8; delta++)
                            {
                                int g = (j - 1) * 8 + delta;

                                //找到科目
                                if (ans.teacher[t, g - 1].id == ans.teacher[t, g2].id)
                                {
                                    if (course_week_sum[ans.teacher[t, g].cno] < 5 && !SwapConflict(ref ans, t, g, g2))
                                    {
                                        //成功交换
                                        Swap<Tea>(ref ans.teacher[t, g], ref ans.teacher[t, g2]);

                                        if (course_sum.ContainsKey(ans.teacher[t, g].id))
                                        {
                                            course_sum[ans.teacher[t, g].id]++;
                                        }
                                        else
                                        {
                                            course_sum.Add(ans.teacher[t, g].id, 1);
                                        }

                                        findsolution = true;
                                    }
                                    else
                                    {
                                        //不是副科
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!findsolution)
                    {
                        //之后//未优化
                        for (int j = i + 1; j <= 5; j++)
                        {
                            int count = 0;
                            for (int delta = 1; delta <= 8; delta++)
                            {
                                int g = (j - 1) * 8 + delta;
                                if (ans.teacher[t, g].id == ans.teacher[t, g2].id)
                                {
                                    count++;
                                }
                            }
                            if (count >= 2)
                            {
                                //已经有两节课以上
                                continue;
                            }

                            //随意和星期j的非主课调换
                            for (int g = (j - 1) * 8 + 1; g <= (j - 1) * 8 + 8; j++)
                            {
                                if (course_week_sum[ans.teacher[t, g].cno] < 5 && !SwapConflict(ref ans, t, g, g2))
                                {
                                    //成功交换
                                    findsolution = true;
                                    Swap<Tea>(ref ans.teacher[i, g], ref ans.teacher[i, g2]);

                                    if (course_sum.ContainsKey(ans.teacher[t, g].id))
                                    {
                                        course_sum[ans.teacher[t, g].id]++;
                                    }
                                    else
                                    {
                                        course_sum.Add(ans.teacher[t, g].id, 1);
                                    }

                                    break;
                                }
                            }

                            if (!findsolution)
                            {

                            }
                        }
                    }

                    if (!findsolution)
                    {
                        //
                        //实际应该不会发生
                        //
                    }
                }
            }

            //语文考虑完后
            //course_sum.Remove(yuwentch);
            //这时候语文数据不一定正确

            //考虑其他课连堂
            /*foreach (var item in course_sum)
            {
                if (item.Key == yuwentch) continue;

                if (item.Value > 2)//同一天3节课或以上。。。
                {

                }
                else if (item.Value == 2)//同一天两节课就连堂
                {
                    //定位这两节课
                    int g1 = 0, g2 = 0;
                    for (int j = 1; j <= 8; j++)
                    {
                        int g = (i - 1) * 8 + j;//在1~40哪一位
                        if (ans.teacher[t, g].id == item.Key)
                        {
                            if (g1 == 0)
                            {
                                g1 = g;
                            }
                            else
                            {
                                g2 = g;
                                break;
                            }
                        }
                    }

                    if (g2 - g1 == 1)
                    {
                        //已经连堂
                        continue;
                    }

                    //在当天移动一个产生连堂情况
                    bool findsolution = false;//是否有这样的连堂情况
                    //不破坏其他连堂的课//也不和语文交换
                    if (course_sum[ans.teacher[t, g2 - 1].id] == 1 && ans.teacher[t, g2 - 1].id != yuwentch && !SwapConflict(ref ans, t, g2 - 1, g1))
                    {
                        //g1和g2-1交换
                        //findsolution = true;
                        Swap<Tea>(ref ans.teacher[t, g1], ref ans.teacher[t, g2 - 1]);
                    }
                    else if (course_sum[ans.teacher[t, g1 + 1].id] == 1 && ans.teacher[t, g1 + 1].id != yuwentch && !SwapConflict(ref ans, t, g2, g1 + 1))
                    {
                        //g1+1和g2交换
                        //findsolution = true;
                        Swap<Tea>(ref ans.teacher[t, g2], ref ans.teacher[t, g1 + 1]);
                    }
                    else if ((g1 - 1) % 8 + 1 >= 2)
                    {
                        //g1-1 g2;
                        if (course_sum[ans.teacher[t, g1 - 1].id] == 1 && ans.teacher[t, g1 - 1].id != yuwentch && !SwapConflict(ref ans, t, g2 , g1- 1))
                        {
                            Swap<Tea>(ref ans.teacher[t, g2], ref ans.teacher[t, g1 - 1]);
                        }
                    }
                    else if ((g2 - 1) % 8 + 1 <= 7)
                    {
                        //g1,g2+1
                        if(course_sum[ans.teacher[t, g2 + 1].id] == 1 && ans.teacher[t, g2 + 1].id != yuwentch && !SwapConflict(ref ans, t, g2+1, g1))
                        {
                            Swap<Tea>(ref ans.teacher[t, g1], ref ans.teacher[t, g2 + 1]);
                        }
                    }

                    if(!findsolution)
                    {
                        //安排到其他天

                    }

                }
            }*/
        }
    }

    //模拟手工调课
    private void ChangeSchedule()
    {
        Random_Permutation RP = new Random_Permutation(N);
        int[] seq = RP.NextRandom();
        for (int t = 1; t <= N; t++)
        {
            //随机顺序调整班级课表
            ChangeSchedule(seq[t - 1] + 1);
        }
    }

    private bool solving()
    {
        int BESTFITNESS = 100 + 100 * N;
        return ans.fit >= BESTFITNESS * 90 / 100;//90%满意
    }

    public void MakingSchedule()
    {
        //产生初始种群//无冲突
        init();

        int loop = 0;//循环次数//遗传深度
        int maxloop = 500;//限定最大循环次数//避免死循环
        while (loop++ < maxloop)
        {
            //评估fit值//更新操作
            Estimate();//.fit
            //如果找到满意解
            if (solving())
            {
                break;
            }
            //遗传操作//产生后代
            Generating();
        }
        //ChangeSchedule();//模拟手工调课

        //得到ans.t_id[][] 
    }

    //对于ans课表i班j节课 所能无冲突调换的 点
    public List<int> givepoint(int i, int j)
    {
        List<int> p = new List<int>();
        p.Clear();
        for (int t = 1; t <= M; t++)
        {
            if (t == j) continue;

            //交换之后均无冲突
            if (!SwapConflict(ref ans, i, j, t))
            {
                p.Add(t);
            }
            /*if (Conflict(ref ans, i, t, ans.teacher[i, j].id) == 0 && Conflict(ref ans, i, j, ans.teacher[i, t].id) == 0)
            {
                p.Add(t);
            }*/
        }
        return p;
    }

    public void changeclass(int t, int i, int j)
    {
        //ans
        //t班i,j节课互换
        Swap<Tea>(ref ans.teacher[t, i], ref ans.teacher[t, j]);
    }

    public Tea[,] func()
    {
        return ans.teacher;
    }
}