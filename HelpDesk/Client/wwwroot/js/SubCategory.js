$.ajax({
    url: 'https://localhost:7024/api/category/',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    method: 'GET',
    dataType: 'json',
    success: function (data) {
        console.log(data);
        const dropdown = document.getElementById('myDropdown');
        data.data.forEach(category => {
            const option = document.createElement('option');
            option.value = category.guid;
            option.text = category.categoryName;
            dropdown.appendChild(option);
        });
    },
    error: function (error) {
        console.error('Error:', error);
    }
});