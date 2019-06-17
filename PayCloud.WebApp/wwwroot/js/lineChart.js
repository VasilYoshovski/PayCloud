var mylinedata;
var mylinemax = 10000;
var mylinelabels;

function SetupLineChart() {
    Chart.defaults.global.defaultFontFamily = 'Poppins, sans-serif', Chart.defaults.global.defaultFontColor = "#292b2c";

    var config = {
        type: "line",
        data: {
            labels: mylinelabels,
            datasets: [{
                label: "Balance",
                lineTension: .7,
                backgroundColor: "rgba(2,117,216,0.2)",
                borderColor: "rgba(2,117,216,1)",
                pointRadius: 5,
                pointBackgroundColor: "rgba(2,117,216,1)",
                pointBorderColor: "rgba(255,255,255,0.8)",
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(2,117,216,1)",
                pointHitRadius: 20,
                pointBorderWidth: 2,
                data: mylinedata
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: "date"
                    },
                    gridLines: {
                        display: !1
                    },
                    ticks: {
                        maxTicksLimit: 7,
                        fontSize: 11
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: mylinemax,
                        maxTicksLimit: 5,
                        fontSize: 11
                    },
                    gridLines: {
                        color: "rgba(0, 0, 0, .125)"
                    }
                }]
            },
            legend: {
                display: !1
            }
        }
    };
    return config;
}
//window.onload = function () {
//    getLineChartData(3);
//    getChartData();

//};

function getLineChartData(accountId, update) {
    $.ajax({
        url: '/user/accounts/GetLineChartInfo?accountId=' + accountId,
        type: 'GET',
        cache: false,
        dataType: 'json',
        success: function (data) {
            console.log(data);
            mylinedata = data.map(x => x.balance);
            console.log(mylinedata)
            mylinelabels = data.map(x => x.date);
            mylinelabels[mylinelabels.length - 1] = "Now";
            var arraymax = Math.max(...mylinedata);
            mylinemax = arraymax + Math.round(arraymax * 0.1);
            console.log(mylinelabels);
            //if (update) {
            //    console.log("update-----")
            //    myPie.data.datasets[0] = {
            //        data: mylinedata,
            //        borderColor: ['rgba(255,255,255,0.5)'],
            //        borderWidth: 1,
            //        label: 'PayCloud pie chart'
            //    };

            //    myPie.data.labels = mylinelabels;
            //    myPie.options.animation.animateRotate = false;
            //    myPie.options.animation.animateScale = false;

            //    myPie.update();
            //}
            //else 
            {

                var ctxline = document.getElementById('myAreaChart');
                window.myLine = new Chart(ctxline, SetupLineChart());
            }


        }
    });
};
