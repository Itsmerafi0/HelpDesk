function encodeFile(input) {
    const file = input.files[0];
    const reader = new FileReader();

    reader.onloadend = function () {
        const bytes = new Uint8Array(reader.result);
        const encodedString = btoa(String.fromCharCode.apply(null, bytes));
        input.previousElementSibling.value = encodedString;
    }

    if (file) {
        reader.readAsArrayBuffer(file);
    }
}


$.ajax({
    url: 'https://localhost:7024/api/SubCategory/Detail',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    method: 'GET',
    dataType: 'json',
    success: function (data) {
        console.log(data);
        const dropdown = document.getElementById('myDropdown');
        data.data.forEach(subCategory => {
            const option = document.createElement('option');
            option.value = subCategory.guid;
            option.text = subCategory.name;
            dropdown.appendChild(option);
        });
    },
    error: function (error) {
        console.error('Error:', error);
    }
});

