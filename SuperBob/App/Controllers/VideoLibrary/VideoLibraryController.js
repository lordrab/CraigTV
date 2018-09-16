
app.controller("videoLibraryCtrl", function ($scope, $http, $uibModal) {

    $scope.CustomAdd = function (index, rowIndex, superListAddToModel) {        

    var modalId = $uibModal.open({
        template: `<div class="modal-content">
                        <div class="modal-header">Upload Video</div>
                         <div class="modal-body"><div class="container">
                         <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-12">
                                <label>Name</name>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                <input type="text" class="form-control" ng-model="model.videoName"/>
                                 </div>
                                </div>
                                </div>
                            </div>
                         </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-12">
                                <label>Video Genre</name>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                <select class="form-control" ng-model="model.genreType">
                                <option ng-repeat="x in genreData" value="{{x.id}}">{{x.genreType}}</option>
                                </select>
                                </div>
                                </div>
                                </div>
                            </div>
                         </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-12">
                                <label>Description</name>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                <textarea class="form-control" ng-model="model.videoDescription">
                                </textarea>
                                </div></div>
                                </div>
                            </div>
                         </div>
                          <div class="row">
                           <div class="col-sm-9"><input type="file" id="file" name="file"  class="inputfile" onchange="angular.element(this).scope().uploadFile(this.files)"/>
                           </div>
                          </div>                           
                             <div class="row">
                                <div class="col-sm-3">
                                    Upload File
                                </div>
                                <div class="col-sm-5">
                                    {{uploadFileName}}
                                </div>
                                <div class="col-sm-4">
                                        <button style="height: 26px;">
                                        <label for="file" >Browse</label></button>
                                                                         
                                </div>
                            </div>                       
                        <div class="row">
                            <div class="col-sm-12">
                                <br/>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped active" role="progressbar" id="uploadbar" aria-valuenow="20" aria-valuemin="100" aria-valuemax="300" style="width:0%">
                                    <span id="progressbar-label">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div style="float: right;">
                                <button ng-click="Upload()">Upload</button>  
                                <button ng-click="cancelUpload()" ng-disabled="disableCancel">Cancel</button>
                                </div>
                            </div>
                        </div>
                        </div></div>
                        </div>`
        ,
        backdrop: 'static',
        keyboard: false,
        controller: function ($scope) {
            $scope.disableCancel = false;
            $scope.model = { fileName: '', videoName: '', genreType: 1, videoDescription: ''}
            $scope.genreData = [];
            $scope.uploadFileName = 'Click Browse to select file....';
            
            $http({
                url: '/VideoLibrary/GetGenreList',
                method: 'get'
            }).then(function (response) {
                for (i = 0; i < response.data.length; i++) {
                    var rowModel = { id: response.data[i].GenreId, genreType: response.data[i].GenreType}
                    $scope.genreData.push(rowModel)
                }                
            })

            if (rowIndex != 0) {
                console.log(index)
                var data = { id: index };
                $http({
                    url: '/VideoLibrary/EditData',
                    method: 'post',
                    data: data
                }).then(function (response) {
                    console.log(response)
                    $scope.model.videoName = response.data.Model.Name;
                    $scope.model.fileName = response.data.Model.FileName;
                    $scope.model.genreType = response.data.Model.GenreId;
                   // $scope.model.videoDescription = response.data.Model.GenreId;
                })
            }

            $scope.uploadFile = function (test) {
                $scope.uploadFileName = test[0].name;
                // now trigger a view model update
                $scope.model.fileName = test[0].name;
                if ($scope.model.videoName == "") {
                    $scope.model.videoName = test[0].name.slice(0, -4);
                }               
                $scope.$apply();
            }

            $scope.Upload = function () {

                $scope.disableCancel = true;
                var files = document.getElementById('file').files;
                var file = files[0];

                $scope.fileSize = file.size;

                var filetype = file.type;
                var filename = file.name
                var block = 6000000;
                var filePointer = 0;
                var fileLock = false;
                var writeSuccess = false;
                var endOfFile = false;

                $scope.uploaded = 0;

                var refreshIntervalId = setInterval(function () {

                    if ($scope.fileSize < block) {
                        block = $scope.fileSize;
                    }
                    //console.log($scope.fileSize)
                    //console.log($scope.uploaded)
                    if ($scope.uploaded < $scope.fileSize) {

                        if (fileLock === false) {
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
                                                filePointer = filePointer + block;
                                            }
                                            var percent = $scope.uploaded / $scope.fileSize * 100
                                            
                                            $("#uploadbar").css("width", percent + "%")
                                            $("#progressbar-label").text(Math.round(percent) + "%")
                                            $("#bob").text(filePointer)
                                            if (endOfFile != true) {
                                                fileLock = false;
                                            } else {                                             
                                                //clearInterval(refreshIntervalId);  not needed !!                                            
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
                        
                            $http({
                                url: '/VideoLibrary/AddEditVideoLibrary',
                                method: 'post',
                                data: $scope.model
                            }).then(function (response) {
                                //superListModel.push(response.data.displayListModel)
                                var saveModel = {
                                    Id: response.data.displayListModel.Id, GenreType:  response.data.displayListModel.GenreType,
                                    GenreId: response.data.displayListModel.GenreType, Name:  response.data.displayListModel.Name                                      
                                }
                                superListAddToModel(saveModel);
                                modalId.close();
                            })                       
                    }
                }, 1000);
            };

            function getB64Str(buffer) {
                var binary = '';
                var bytes = new Uint8Array(buffer);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                return window.btoa(binary);
            }

            $scope.cancelUpload = function () {                
                modalId.close();
            };
        },
        scope: $scope
    });
}

})



