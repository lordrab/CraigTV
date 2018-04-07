app.directive('superList', function (dataService) {
    return {

        template: `<div class="container"><div class="row">
                <button class="btn btn-primary addButton" ng-click="AddRecord(0)">Add</button>

                </div>
                <div class="row">
                <table class="table table-bordered"><tr >
                <th ng-repeat="property in propertyNames" ng-show="property.DisplayProperty">{{property.DisplayName}}</th>
                <th>Action</th>
                </tr>
                <tr ng-repeat="row in rowData">
                <td ng-repeat="(key,value) in row" ng-if="row[key].display">
                  
                <span >{{row[key].value}}</span>
                </td>
                
                <td><button class="btn btn-success" ng-click="AddRecord(row.Id.value,$index)">Edit</button>
                <button class="btn btn-danger">Delete</button>
                </td>

                </tr>
                
                </table>
                </div>
        <div class="row">{{data.Id}}</div>
</div>
`,
        controller: function SuperListController($scope, $http, $uibModal) {

            $scope.propertyNames = [];
            $scope.rowData = [];
            $scope.currentEditId = 0;

            $scope.AddRecord = function (index,rowIndex) {
                console.log(index)
                $scope.currentEditId = rowIndex;
                $http({
                    url: $scope.superListData.addEditUrl,
                    method: 'post',
                    data: { Id: index }
                }).then(function (result) {

                    var modal = {
                        popupModal: result.data.PopupDisplayModel,
                        dataModel: result.data.Model
                    }
                    $scope.DataModal(modal, $scope);
                })

            }

            
            $scope.form = "";
            $http({
                url: $scope.superListData.displayUrl,
                method: 'get'
            }).then(function (response) {
                
                $scope.propertyNames = response.data.PropertyNames;

                for (i = 0; i < response.data.DataList.length; i++) {

                    $scope.createListModel(response.data.DataList[i])

                }

            });

            $scope.createListModel = function (data) {
                var model = {};
                for (p = 0; p < $scope.propertyNames.length; p++) {

                    var value = data[$scope.propertyNames[p].PropertyName];

                    model[$scope.propertyNames[p].PropertyName] = {
                        value: value,
                        display: $scope.propertyNames[p].DisplayProperty
                    }
                }
                console.log(model)

                $scope.rowData.push(model);

            }

            $scope.DataModal = function (frameWorkModel, scope) {

                var modalId = $uibModal.open({
                    template: `<div class="modal-content">
                        <div class="modal-header">Test</div>
                        <div class="modal-body"><div class="container">
                        <div class="row craig" ng-repeat="row in uiPopUpModel">
                        <div ng-if="row.tagType == 1">
                        <div class="col-sm-3">{{row.label}}</div>                        
                        <div class="col-sm-9"><input type="text" ng-model="row.Value"/></div>
                        </div>
                        <div ng-if="row.tagType == 0" hidden>
                        <div class="col-sm-3">{{row.label}}</div>                        
                        <div class="col-sm-9"><input type="text" ng-model="row.Value"/></div>
                        </div>                      
                        <div ng-if="row.tagType == 3" >
                        <div class="col-sm-3">{{row.label}}</div>                       
                        <div class="col-sm-9">
                        <select ng-model="row.listId" ng-options="option.Text for option in row.Value track by option.Value">                        
                        </select>
                        </div>
                        </div> </div>
                        <div class="modal-footer"><button class="btn btn-success" ng-click="SaveData()">Save</button>
                        <button class="btn btn-default" ng-click="ClosePersonModal()">Cancel</button>
                        </div>
                        </div>`
                    ,
                    controller: function ($scope) {

                        // build popup modal object    
                        
                        $scope.uiPopUpModel = {};
                        for (i = 0; i < frameWorkModel.popupModal.length; i++) {
                            if (frameWorkModel.popupModal[i].PropertyName.indexOf("List") != -1) {
                                var linkListId = frameWorkModel.popupModal[i].PropertyName.replace("List", "Id");
                                var linkedListIdValue = frameWorkModel.dataModel[linkListId].toString();
                            }

                            $scope.uiPopUpModel[frameWorkModel.popupModal[i].PropertyName] = {
                                Value: frameWorkModel.dataModel[frameWorkModel.popupModal[i].PropertyName],
                                label: frameWorkModel.popupModal[i].DisplayName,
                                tagType: frameWorkModel.popupModal[i].TagType,
                                listId: { Value: linkedListIdValue, linkedId: linkListId }
                            };

                        }

                        $scope.ClosePersonModal = function () {
                            modalId.close();
                        };

                        $scope.SaveData = function () {
                            // model object used in viewmodel. label is what is displayed as label. model is property in ng-model
                            var returnModel = {};

                            // get keys including property names from object
                            var totalKeys = Object.keys($scope.uiPopUpModel);


                            // build object with just property names and values to send to controller
                            for (i = 0; i < totalKeys.length; i++) {

                                switch (typeof ($scope.uiPopUpModel[totalKeys[i]].Value)) {
                                    case "object":
                                        databaseId = totalKeys[i].replace("List", "Id")
                                        returnModel[databaseId] = $scope.uiPopUpModel[totalKeys[i]].listId.Value;
                                        break;
                                    default:
                                        returnModel[totalKeys[i]] = $scope.uiPopUpModel[totalKeys[i]].Value;
                                }
                            }

                            $http({
                                method: 'post',
                                url: scope.superListData.saveDataUrl,
                                data: returnModel
                            }).then(function (result) {
                               console.log(result)
                                if (result.data.Success) {
                                    if (result.data.AddRecord) {
                                        var newObj = {};
                                        for (i = 0; i < result.data.AddDisplayListData.length; i++) {
                                            newObj[result.data.AddDisplayListData[i].PropertyName] = result.data.AddDisplayListData[i].PropertyValue;
                                        }
                                        scope.createListModel(newObj)
                                    } else {
                                        var id = scope.currentEditId;
                                        console.log(id)
                                        console.log(scope.rowData)
                                        for (i = 0; i < result.data.AddDisplayListData.length; i++) {
                                            scope.rowData[id][result.data.AddDisplayListData[i].PropertyName].value = result.data.AddDisplayListData[i].PropertyValue;
                                        }
                                    }
                                   
                                    
                                };


                                modalId.close();
                            });

                        };
                    }
                });

            };
        }
    }
});
