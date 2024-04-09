
// VietNam map
(async () => {

    // Prepare random data
    const data = [
        ['Hà Nội', 728],
        ['Bắc Giang', 710],
        ['Thái Nguyên', 963],
        ['Cao Bằng', 541],
        ['Khánh Hoà', 622],
        ['Quảng Ninh', 866],
        ['Điện Biên', 398],
        ['Đà Nẵng', 985],
        ['Vĩnh Phúc', 223],
        ['Hoà Bình', 605],
        ['Nam Định', 237],
        ['Hải Phòng', 157],
        ['Nghệ An', 134],
        ['Thừa Thiên Huế', 136],
        ['Thanh Hoá', 704],
        ['Bắc Ninh', 361]
    ];

    // Load the geojson germany map
    const geojson = await fetch(
        'https://res.cloudinary.com/pv-duc/raw/upload/v1626132866/province.json?fbclid=IwAR2NWEQj6xqGBiV0EkH-gRjT_jRXYhUqqEWFoNEkwTmREWFix5L-_QhcuN0'
    ).then(response => response.json());

    // Initialize the chart
    Highcharts.mapChart('vietnam_map_1', {
        chart: {
            map: geojson
        },

        xAxis: {
            min: 20,
            max: 60
        },

        title: {
            text: 'Viet Nam'
        },

        accessibility: {
            typeDescription: 'Map of VietNam.'
        },

        mapNavigation: {
            enabled: true,
            buttonOptions: {
                verticalAlign: 'bottom'
            }
        },

        colorAxis: {
            tickPixelInterval: 100
        },

        series: [{
            data: data,
            keys: ['ten_tinh', 'value'],
            joinBy: 'ten_tinh',
            name: 'Province',
            dataLabels: {
                enabled: true,
                format: '{point.key}'
            },
            tooltip: {
                pointFormat: '{point.properties.ten_tinh}: {point.value}'
            }
        }]
    });
})();


// Column chart (Most province)
document.addEventListener("DOMContentLoaded", function () {
    var label = ["Hà Nội", "Nghệ An", "Thanh Hoá", "Bắc Giang", "Vĩnh Phúc", "Điện Biên"];
    var data = [100, 80, 34, 55, 32, 11];
    // Bar chart
    new Chart(document.getElementById("chartjs-dashboard-bar"), {
        type: "bar",
        data: {
            labels: label,
            datasets: [{
                label: "This year",
                backgroundColor: window.theme.primary,
                borderColor: window.theme.primary,
                hoverBackgroundColor: window.theme.primary,
                hoverBorderColor: window.theme.primary,
                data: data,
                barPercentage: .75,
                categoryPercentage: .5
            }]
        },
        options: {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                yAxes: [{
                    gridLines: {
                        display: false
                    },
                    stacked: false,
                    ticks: {
                        stepSize: 20
                    }
                }],
                xAxes: [{
                    stacked: false,
                    gridLines: {
                        color: "transparent"
                    }
                }]
            }
        }
    });
});

// Recent Movement chart
document.addEventListener("DOMContentLoaded", function () {
    // Get Data
    var keys = [];
    var values = [];
    $.ajax({
        url: "/Admin/Chart/NewUserPast7",
        type: 'GET',
        success: function (response) {
            if (response && response.data) {
                var data = response.data;
                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        keys.push(key);
                        values.push(data[key]);
                    }
                }

                var ctx = document.getElementById("chartjs-dashboard-line").getContext("2d");
                var gradient = ctx.createLinearGradient(0, 0, 0, 225);
                gradient.addColorStop(0, "rgba(215, 227, 244, 1)");
                gradient.addColorStop(1, "rgba(215, 227, 244, 0)");
                var key = ['26/03', '27/03', '28/03', '29/03', '30/03', '31/03', '01/04']
                console.log(key);
                console.log(keys);
                // Line chart
                new Chart(document.getElementById("chartjs-dashboard-line"), {
                    type: "line",
                    data: {
                        labels: keys,
                        datasets: [{
                            label: "Count",
                            fill: true,
                            backgroundColor: gradient,
                            borderColor: window.theme.primary,
                            data: values
                        }]
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        tooltips: {
                            intersect: false
                        },
                        hover: {
                            intersect: true
                        },
                        plugins: {
                            filler: {
                                propagate: false
                            }
                        },
                        scales: {
                            xAxes: [{
                                reverse: true,
                                gridLines: {
                                    color: "rgba(0,0,0,0.0)"
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    stepSize: 1000
                                },
                                display: true,
                                borderDash: [3, 3],
                                gridLines: {
                                    color: "rgba(0,0,0,0.0)"
                                }
                            }]
                        }
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
    
});

// Calendar chart
document.addEventListener("DOMContentLoaded", function () {
    var today = new Date(); // Get current date
    var defaultDate = today.getFullYear() + "-" + (today.getMonth() + 1) + "-" + today.getDate();

    document.getElementById("datetimepicker-dashboard").flatpickr({
        inline: true,
        prevArrow: "<span title=\"Previous month\">&laquo;</span>",
        nextArrow: "<span title=\"Next month\">&raquo;</span>",
        defaultDate: defaultDate,
        dateFormat: "Y-m-d", // Format of the default date
        onReady: function (selectedDates, dateStr, instance) {
            // Add 'selected' class to the current date
            instance.selectedDates.push(today);
            instance.redraw();
        }
    });
});


