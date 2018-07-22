app.directive('superList', function (yesNoPopupService) {
    return {
        scope: {
            mvccontroller: '@',
            listDataMethod: '@',
            saveDataMethod: '@',
            deleteDataMethod: '@',
            addFunction: '='
        },
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
                <button class="btn btn-danger" ng-click="DeleteRecord(row.Id.value,$index)">Delete</button>
                <div class="test"></div>
                </td>
                
                </tr>               
                </table>
                </div>
        <div class="row">{{data.Id}}</div>
</div>
`,
        controller: function SuperListController($scope, $http, $uibModal) {
            
            $scope.superListData = {
                displayUrl: '/' + $scope.mvccontroller + '/DisplayList',
                addEditUrl: '/' + $scope.mvccontroller + '/EditData',
                saveDataUrl: '/' + $scope.mvccontroller + '/SaveCurrentData',
                deleteDataUrl: '/' + $scope.mvccontroller + '/Delete',
                addFunction: $scope.addFunction
                
            }
            
            // if as different display list method has been passed use it
            if (typeof($scope.listDataMethod) != 'undefined') {
                $scope.superListData.displayUrl = '/' + $scope.mvccontroller + '/' + $scope.listDataMethod;
            } 
            // same with a delete method
            if (typeof ($scope.deleteDataMethod) != 'undefined') {
                $scope.superListData.deleteDataUrl = '/' + $scope.mvccontroller + '/' + $scope.deleteDataMethod;
            }

            // same with a delete method
            if (typeof ($scope.saveDataMethod) != 'undefined') {
                $scope.superListData.saveDataUrl = '/' + $scope.mvccontroller + '/' + $scope.saveDataMethod;
            }
            $scope.propertyNames = [];
            $scope.rowData = [];
            $scope.currentEditId = 0;

            $scope.AddRecord = function (index, rowIndex) {
                console.log(typeof ($scope.addFunction))
                if (typeof ($scope.addFunction ) != "undefined"  ) {
                    
                    $scope.superListData.addFunction(index, rowIndex, $scope.createListModel, $scope.getRowData);                   
                   
                } else {   
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
            };

            $scope.DeleteRecord = function (index, rowIndex) {


                var DeleteFunction = function (url, index) {
                    $http({
                        method: 'post',
                        url: url,
                        data: { id: index }
                    }).then(function (result) {

                        if (result.data.Success) {
                            $scope.rowData.splice(rowIndex, 1)
                        }

                    });
                }

                yesNoPopupService.YesNoPopup(function () { DeleteFunction($scope.superListData.deleteDataUrl, index); });

            };

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
                $scope.rowData.push(model);                
            };

            $scope.getRowData = function (rowIndex) {
                return $scope.rowData[rowIndex]
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
                        </div>
                        </div>
                        <div class="modal-footer"><button class="btn btn-success" ng-click="SaveData()">Save</button>
                        <button class="btn btn-default" ng-click="ClosePersonModal()">Cancel</button>
                        
                        </div>`
                    ,
                    controller: function ($scope) {

                        $("#file").css('opacity','0');
                        
                        // build popup modal object                           
                        $scope.uiPopUpModel = {};
                        $scope.fileSize = 0;
                        $scope.uploaded = 0;
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

                        this.handlechange = function(e) {
                            console.log(e)
                        }

                        
                        $scope.Upload = function () {

                            var files = document.getElementById('file').files;
                            var file = files[0];

                            $scope.fileSize = file.size;

                            var filetype = file.type;
                            var filename = file.name
                            var block = 3000000;
                            var filePointer = 0;
                            var fileLock = false;
                            var writeSuccess = false;
                            var endOfFile = false;
                            
                            console.log(file)

                            $scope.uploaded = 0;
                           
                            var refreshIntervalId = setInterval(function () {                              

                                if ($scope.fileSize < block) {
                                    block = $scope.fileSize;
                                }

                                if ($scope.uploaded < $scope.fileSize) {
                                    
                                    if (fileLock == false) {
                                        fileLock = true;
                                        var leftToDownload = $scope.fileSize - $scope.uploaded;
                                        
                                        if (leftToDownload < block) {
                                            block = leftToDownload;
                                            endOfFile = true;
                                        }
                                        var blob = file.slice(filePointer, filePointer + block);
                                        var fileData = new FileReader();

                                        fileData.onloadend = function (evt) {
                                            if (evt.target.readyState == FileReader.DONE) {
                                                var data = { fileName: filename, fileData: getB64Str(evt.target.result), contentType: "ss", endOfFile: endOfFile }
                                                
                                                $.ajax({
                                                    url: '/VideoLibrary/Upload',
                                                    type: 'post',
                                                    data: data,
                                                    success: function (data) {                                                        
                                                        writeSuccess = data.writeSuccess;
                                                        if (writeSuccess) {                                                            
                                                            $scope.uploaded = filePointer + block;
                                                            filePointer = filePointer + block;                                                        }
                                                        var percent = $scope.uploaded / $scope.fileSize * 100
                                                        console.log(percent)
                                                        $("#uploadbar").css("width", percent + "%")
                                                        $("#progressbar-label").text(Math.round(percent) + "%")
                                                        $("#bob").text(filePointer)
                                                        if (endOfFile != true) {
                                                            fileLock = false;
                                                        } else {
                                                            clearInterval(refreshIntervalId);
                                                        }

                                                    },
                                                    error: function (response) {
                                                        console.log(response)
                                                    }
                                                })
                                            }
                                        }

                                        fileData.readAsArrayBuffer(blob);    
                                    }
                                } else {
                                    clearInterval(refreshIntervalId);
                                    console.log("End")
                                    console.log($scope.uploaded)
                                }
                            }, 1000);

                        }
                        function getB64Str(buffer) {
                            var binary = '';
                            var bytes = new Uint8Array(buffer);
                            var len = bytes.byteLength;
                            for (var i = 0; i < len; i++) {
                                binary += String.fromCharCode(bytes[i]);
                            }
                            return window.btoa(binary);
                        }

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
