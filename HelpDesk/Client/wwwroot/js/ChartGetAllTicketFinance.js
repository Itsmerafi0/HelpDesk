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
                        position: 'top'
                    }
                }
            }
        });
    },
    error: function (xhr, status, error) {
        console.log('Error:', error);
    }
});
