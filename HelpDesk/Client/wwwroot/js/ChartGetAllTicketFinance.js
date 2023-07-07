$.ajax({
    url: 'https://localhost:7024/api/ticket/ticketdetailfinance',
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