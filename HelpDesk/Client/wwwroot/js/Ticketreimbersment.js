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
    url: 'https://localhost:7024/api/subcategory/detail',
    headers: {
        'Authorization': 'Bearer ' + jwToken
    },
    method: 'GET',
    dataType: 'json',
    success: function (data) {
        console.log(data);
        const dropdown = document.getElementById('myDropdown');
        const categoryName = 'Reimbursement'; // Ganti dengan nama kategori yang diinginkan (misalnya 'access' atau 'reimbursement')
        data.data.forEach(subCategory => {
            if (subCategory.categoryName === categoryName) {
                const option = document.createElement('option');
                option.value = subCategory.guid;
                option.text = subCategory.name;
                dropdown.appendChild(option);
            }
        });
    },
    error: function (error) {
        console.error('Error:', error);
    }
});


