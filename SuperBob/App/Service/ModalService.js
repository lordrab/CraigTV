app.service("dataService", function ($http, $uibModal) {


    

    this.AddToListModel = function (listInputData,scope) {
        scope.propertyNames = listInputData.data.PropertyNames;

        // $scope.rowData = response.data.DataList;
        for (i = 0; i < listInputData.data.DataList.length; i++) {
            var dataKeys = Object.keys(listInputData.data.DataList[i])
            var newRow = {};
            for (k = 0; k < dataKeys.length; k++) {

                var display = true;
                for (p = 0; p < listInputData.data.PropertyNames.length; p++) {
                    if (listInputData.data.PropertyNames[p].PropertyName == dataKeys[k]) {
                        display = listInputData.data.PropertyNames[p].DisplayProperty
                    }
                }
                newRow[dataKeys[k]] = {
                    value: listInputData.data.DataList[i][dataKeys[k]],
                    display: display
                }
            }

            scope.rowData.push(newRow)

        }              

    }
});