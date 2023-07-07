fetch('https://localhost:7024/api/Complain')
    .then(response => response.json())
    .then(data => {
    const labels = Object.keys(data);
    const values = Object.values(data);

    const ctx = document.getElementById('pieChart').getContext('2d');
    const chart = new Chart(ctx, {
        type: 'pie',
    data: {
        labels: labels,
    datasets: [{
        data: values,
    backgroundColor: [
    '#FF6384',
    '#36A2EB',
    '#FFCE56',
    // Add more colors as needed
    ],
            }],
            },
    options: {
        // Specify chart options as needed
    },
    });
    });