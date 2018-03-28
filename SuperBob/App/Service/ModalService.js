app.service("dataService", function ($http, $uibModal) {

    this.DataModal = function (dataObj) {

        var modalId = $uibModal.open({
            templateUrl: dataObj.templateUrl,
            controller: function ($scope) {
                $scope.personData = dataObj.currentRecord;
                $scope.ClosePersonModal = function () {
                    modalId.close();
                };
                $scope.SaveData = function () {
                    $http({
                        method: 'post',
                        url: '/Home/SaveData',
                        data: $scope.personData
                    }).then(function (result) {
                        console.log(dataObj.currentIndex)
                        if (result.data.success) {
                            var props = Object.keys(dataObj.currentRecord);
                            if (dataObj.currentIndex != -1) {
                                for (i = 0; i < props.length; i++) {

                                    dataObj.dataList[dataObj.currentIndex][props[i]] = $scope.personData[props[i]];
                                }
                            } else {
                                console.log("dadda")
                            }
                        }


                        modalId.close();
                    });

                };
            }
        });

    };
});