$.ajax({
    url: 'https://localhost:7024/api/ticket/ticketdetail',
    method: 'GET',
    dataType: 'json',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    success: function (dataFromAPI) {
        // Calculate total count of each risk level
        const riskLevels = {
            Low: 0,
            Medium: 0,
            High: 0
        };

        dataFromAPI.data.forEach(item => {
            switch (item.riskLevel) {
                case 0:
                    riskLevels.Low++;
                    break;
                case 1:
                    riskLevels.Medium++;
                    break;
                case 2:
                    riskLevels.High++;
                    break;
            }
        });

        // Create a chart using Chart.js
        const ctx = document.getElementById('myChart').getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Low', 'Medium', 'High'],
                datasets: [
                    {
                        label: 'Risk Level',
                        data: [riskLevels.Low, riskLevels.Medium, riskLevels.High],
                        backgroundColor: ['rgb(0, 209, 205)', 'rgb(255, 205, 86)', 'rgb(255, 99, 132)'],
                        borderWidth: 1,
                    },
                ]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            display: true,
                            color: 'rgba(0, 0, 0, 0.1)'
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'Chart Data Risk Level'
                    },
                    legend: {
                        display: true, // Display legend
                        position: 'top',
                        labels: {
                            generateLabels: function (chart) {
                                // Generate custom legend labels based on the data
                                const data = chart.data;
                                if (data.labels.length && data.datasets.length) {
                                    return data.labels.map((label, index) => {
                                        const dataset = data.datasets[0];
                                        const backgroundColor = dataset.backgroundColor[index];
                                        return {
                                            text: label + ': ' + dataset.data[index],
                                            fillStyle: backgroundColor,
                                            hidden: false,
                                            lineCap: 'butt',
                                            lineDash: [],
                                            lineDashOffset: 0,
                                            lineJoin: 'miter',
                                            lineWidth: 1,
                                            strokeStyle: backgroundColor,
                                            pointStyle: 'circle',
                                            rotation: 0
                                        };
                                    });
                                }
                                return [];
                            }
                        }
                    }
                }
            }
        });
    },
    error: function (xhr, status, error) {
        console.log('Error:', error);
    }
});

$.ajax({
    url: 'https://localhost:7024/api/ticket/ticketdetail',
    method: 'GET',
    dataType: 'json',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    success: function (dataFromAPI) {
        // Calculate total count of each risk level
        const riskLevels = {
            Low: 0,
            Medium: 0,
            High: 0
        };

        const totalTickets = dataFromAPI.data.length;

        dataFromAPI.data.forEach(item => {
            switch (item.riskLevel) {
                case 0:
                    riskLevels.Low++;
                    break;
                case 1:
                    riskLevels.Medium++;
                    break;
                case 2:
                    riskLevels.High++;
                    break;
            }
        });

        // Calculate percentage
        const percentage = {
            Low: ((riskLevels.Low / totalTickets) * 100).toFixed(2),
            Medium: ((riskLevels.Medium / totalTickets) * 100).toFixed(2),
            High: ((riskLevels.High / totalTickets) * 100).toFixed(2),
        };

        // Create a chart using Chart.js
        const ctx = document.getElementById('chartPie').getContext('2d');
        new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Low', 'Medium', 'High'],
                datasets: [
                    {
                        label: 'Risk Level',
                        data: [riskLevels.Low, riskLevels.Medium, riskLevels.High],
                        backgroundColor: ['rgb(0, 209, 205)', 'rgb(255, 205, 86)', 'rgb(255, 99, 132)'],
                        borderWidth: 1,
                    },
                ]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: 'Chart Data Risk Level'
                    },
                    legend: {
                        display: true, // Display legend
                        position: 'top',
                        labels: {
                            generateLabels: function (chart) {
                                const data = chart.data;
                                if (data.labels.length && data.datasets.length) {
                                    return data.labels.map((label, index) => {
                                        const dataset = data.datasets[0];
                                        const backgroundColor = dataset.backgroundColor[index];
                                        return {
                                            text: label + ': ' + dataset.data[index] + ' (' + percentage[label] + '%)',
                                            fillStyle: backgroundColor,
                                            hidden: false,
                                            lineCap: 'butt',
                                            lineDash: [],
                                            lineDashOffset: 0,
                                            lineJoin: 'miter',
                                            lineWidth: 1,
                                            strokeStyle: backgroundColor,
                                            pointStyle: 'circle',
                                            rotation: 0
                                        };
                                    });
                                }
                                return [];
                            }
                        }
                    }
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            const dataset = data.datasets[tooltipItem.datasetIndex];
                            const currentValue = dataset.data[tooltipItem.index];
                            const percentageValue = percentage[data.labels[tooltipItem.index]];

                            return data.labels[tooltipItem.index] + ': ' + currentValue + ' (' + percentageValue + '%)';
                        }
                    }
                }
            }
        });
    },
    error: function (xhr, status, error) {
        console.log('Error:', error);
    }
});