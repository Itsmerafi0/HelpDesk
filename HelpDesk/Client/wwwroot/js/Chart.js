$.ajax({
    url: 'https://localhost:7024/api/ticket/ticketdetail',
    method: 'GET',
    dataType: 'json',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    success: function (dataFromAPI) {
        // Extracting data for the chart
        const riskLevels = dataFromAPI.data.map(item => item.riskLevel);
        const statusLevels = dataFromAPI.data.map(item => item.statusLevel);

        // Create a chart using Chart.js
        const ctx = document.getElementById('myChart').getContext('2d');
        const myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: dataFromAPI.data.map(item => item.ticketId),
                datasets: [
                    {
                        label: 'Risk Level',
                        data: riskLevels,
                        backgroundColor: 'rgba(255, 99, 132, 0.7)',
                        borderWidth: 1
                    },
                    {
                        label: 'Status Level',
                        data: statusLevels,
                        backgroundColor: 'rgba(54, 162, 235, 0.7)',
                        borderWidth: 1
                    }
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
                        text: 'Risk Level and Status Level'
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