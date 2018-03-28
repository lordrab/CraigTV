app.directive('superList', function () {
    return {
        scope: {
            url: '@'
        },
        template: `<div class="container"><div class="row">
                <button class="btn btn-primary addButton" ng-click="DataModal()">Add</button>
                </div>
                <div class="row">
                <table class="table table-bordered"><tr >
                <th ng-repeat="property in propertyNames">{{property.DisplayName}}</th>
                <th>Action</th>
                </tr>
                <tr ng-repeat="currentRow in rowData">
                <td>{{currentRow.Id}}</td>
                <td>{{currentRow.FirstName}}</td>
                <td>{{currentRow.LastName}}</td>
                <td><button class="btn btn-success">Edit</td>
                </tr>
                </table>
                </div>
        <div class="row">kk</div>
</div>
`,
        controller: function SuperListController($scope, $http, $uibModal) {
            console.log($scope.url)
            $scope.propertyNames = [];
            $scope.rowData = [];
            $scope.form = "";
            $http({
                url: '/User/DisplayList',
                method: 'get'
            }).then(function (response) {
                console.log(response.data)
                $scope.propertyNames = response.data.PropertyNames;
                $scope.rowData = response.data.DataList;

                var form = "";
                for (i = 0; i < response.data.PropertyNames.length; i++) {
                    if (response.data.PropertyNames[i].DisplayProperty) {
                        switch (response.data.PropertyNames[i].TagType) {
                            case 0:
                                $scope.form = $scope.form + '<div class="row"><div class="col-xs-3">' + response.data.PropertyNames[i].DisplayName
                                    + '</div><div class="col-xs-6"> <input type="text"/></div></div>';
                        }
                    }
                }
                             
                var modalHtml1 = ` <div class="model-content">
                    <div class="modal-header">ggffggg</div>
                    <div class="modal-body"><div class="container">`
                var modalHtml2 = $scope.form;

                var modalHtml3 = `</div ></div >

                    <div class="modal-footer"><button class="btn btn-primary">Save</button>
                    <button class="btn btn-default" ng-click="ClosePersonModal()">Cancel</button>
                    </div>

                    
                    </div>
                    `
                $scope.modalLayout = modalHtml1 + modalHtml2 + modalHtml3;
                });

            $scope.DataModal = function () {
                
                var modalId = $uibModal.open({
                    template: $scope.modalLayout,
                    controller: function ($scope) {
                        
                        $scope.ClosePersonModal = function () {
                            modalId.close();
                        };

                        $scope.SaveData = function () {
                            $http({
                                method: 'post',
                                url: '/Home/SaveData',
                                data: $scope.personData
                            }).then(function (result) {
                                
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
        }
    }
})