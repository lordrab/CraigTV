app.service("yesNoPopupService", function ($http, $uibModal) {

    this.YesNoPopup = function (yesFunction) {
        
        var modalId = $uibModal.open({
            template: `<div class="modal-content">
                        <div class="modal-header">Delete Record</div>
                        <div class="modal-body"><div class="container">
                        <div class="row">
                        Do You Want To Delete The Record?</div></div></div>
                        <div class="modal-footer"><button class="btn btn-success" ng-click="Yes()">Yes</button>
                        <button class="btn btn-default" ng-click="No()">No</button>
                        </div>
                        </div>`
            ,
            controller: function ($scope) {
                $scope.yesFunction = yesFunction;                
                $scope.Yes = function () {
                    modalId.close();
                    $scope.yesFunction();
                };

                $scope.No = function () {
                    modalId.close();
                    
                }
            }

        })
        
    }
});