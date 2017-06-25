$('.clockpicker').clockpicker()
            .find('input').change(function () {
                console.log(this.value);
            });