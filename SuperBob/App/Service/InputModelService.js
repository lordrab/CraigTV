app.service("inputModelService", function ($http, $uibModal) {

    this.InputData = function (addFunction, inputLabel) {

        var modalId = $uibModal.open({
            template: `<div class="modal-content">
                        <div class="modal-header">Delete Record</div>
                        <div class="modal-body"><div class="container">
                        <div class="row"><div class="col-sm-12">
                        <label>{{inputLabel}}</label></div></div>
                        <div class="row">
                        <div class="col-sm-12">
                        <input type="text" class="col-sm-12" ng-model="inputData"/>
                        </div>
                        </div></div>
                        <div class="modal-footer"><button class="btn btn-success" ng-click="Save()">Save</button>
                        <button class="btn btn-default" ng-click="Cancel()">Cancel</button>
                        </div>
                        </div>`
            ,
            controller: function ($scope) {
                $scope.inputData = ""
                $scope.inputLabel = inputLabel;
                $scope.Save = function () {
                    modalId.close();
                    addFunction($scope.inputData);
                };

                $scope.Cancel = function () {
                    modalId.close();
                }
            }
        })
    }

    this.dropDownData = function (addFunction, inputLabel,dropdownList) {

        var modalId = $uibModal.open({
            template: `<div class="modal-content">
                        <div class="modal-header">Delete Record</div>
                        <div class="modal-body"><div class="container">
                        <div class="row"><div class="col-sm-12">
                        <label>{{inputLabel}}</label></div></div>
                        <div class="row">
                        <div class="col-sm-12">
                        <select ng-options="x.Id as x.Text for x in dropdownList" ng-model="dropDownId">

                        </select>
                        </div>
                        </div></div>
                        <div class="modal-footer"><button class="btn btn-success" ng-click="Save()">Save</button>
                        <button class="btn btn-default" ng-click="Cancel()">Cancel</button>
                        </div>
                        </div>`
            ,
            controller: function ($scope) {
                $scope.dropDownId = ""
                $scope.inputLabel = inputLabel;               
                $scope.dropdownList = dropdownList;                
                $scope.Save = function () {
                    modalId.close();                  
                    addFunction($scope.dropDownId);
                };

                $scope.Cancel = function () {
                    modalId.close();
                }
            }
        })
    }
});