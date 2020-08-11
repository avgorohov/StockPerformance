// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    function getCompareResults(symbol) {
        let url = [apiBaseUrl, 'v1', 'stocks', symbol, 'comparison'].join('/');

        return $.ajax({
            url: url,
            type: "GET",
            contentType: "application/json"
        });
    }

    function prepareChartDatasets(performanceData) {
        const result = [];
        const colors = ['#4dc9f6',
            '#f67019',
            '#f53794',
            '#537bc4',
            '#acc236',
            '#166a8f',
            '#00a950',
            '#58595b',
            '#8549ba'];

        for (let i = 0; i < performanceData.length; i++) {
            const performanceDataItem = performanceData[i];

            const color = colors[i];

            const data = performanceDataItem.dailyStockPerformances.map(p => ({
                t: new Date(p.dateTime), y: p.value
            }));

            result.push({
                label: performanceDataItem.symbol,
                data: data,
                borderColor: color,
                backgroundColor: color,
                fill: false
            });
        }

        return result;
    }

    function drawPerformanceComparisonCharts(performanceData) {
        var ctx = document.getElementById('stock_perf_chart').getContext('2d');

        datasets = prepareChartDatasets(performanceData);
        var chart = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: datasets
            },
            options: {
                scales: {
                    xAxes: [{
                        type: 'time',
                        time: {
                            unit: 'day'
                        }
                    }]
                }
            }
        });
    }

    $('#show-btn').on('click', function (evt) {
        evt.preventDefault();
        $('#show-btn').prop('disabled', true);

        var symbol = $.trim($('#symbol').val());
        console.log('data -> ', apiBaseUrl, symbol);
        if (!symbol || symbol === '') {
            alert('Please enter correct value');
            return;
        }

        getCompareResults(symbol).then(function (data) {
            drawPerformanceComparisonCharts(data);
            $('#show-btn').prop('disabled', null);
        }, function (err) {
            $('#show-btn').prop('disabled', null);
        });
    })
});
