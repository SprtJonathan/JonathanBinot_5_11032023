function updateSelect(sourceSelectId, targetSelectId, data, idField, parentIdField, selectedFieldId = -1) {
    var sourceId = document.getElementById(sourceSelectId).value;
    var targetSelect = document.getElementById(targetSelectId);
    targetSelect.innerHTML = "";

    if (sourceId !== "") {
        var filteredData = data.filter(function (item) {
            return item[parentIdField] == sourceId;
        });

        filteredData.forEach(function (item) {
            var option = document.createElement("option");
            option.text = item.nom;
            option.value = item[idField];
            console.log(item[idField] + " " + selectedFieldId + " donc " + (selectedFieldId == item[idField]))
            option.selected = selectedFieldId == item[idField] ? "selected" : "";
            targetSelect.appendChild(option);
        });

        targetSelect.disabled = false;
    } else {
        targetSelect.disabled = true;
    }
}