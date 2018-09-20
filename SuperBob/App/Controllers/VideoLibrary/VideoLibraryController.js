
app.controller("videoLibraryCtrl", function ($scope, $http, $uibModal) {

    $scope.filterList = [];
    $scope.selectId = 0;

    $scope.filterModel = {
        filterList: [],
        selectId: 0,
        filterChange: function (filterIndex) {
            console.log(filterIndex)
        }
    }

    $http({
        url: '/VideoLibrary/GetGenreList',
        method: 'get'
    }).then(function (response) {
        console.log(response)
        $scope.filterModel.filterList.push({ Id: 0, Name: 'All' });
        for (i = 0; i < response.data.CatagoryList.length; i++) {
            $scope.filterModel.filterList.push(response.data.CatagoryList[i])
        }
        });


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
                                <input type="text" class="form-control" ng-model="model.VideoName"/>
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
                                <select class="form-control" ng-model="model.GenreType" ng-options="x.Id as x.GenreType for x in genreData"
                                    selected="model.GenreType">
                                
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
                                <label>Video Catagory</name>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                <select class="form-control" ng-model="model.CatagoryName"
                                 ng-options="x.Id as x.Name for x in catagoryData" selected="model.CatagoryName">
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
                                <textarea class="form-control" ng-model="model.VideoDescription">
                                </textarea>
                                </div></div>
                                </div>
                            </div>
                         </div>
                          <div class="row" ng-hide="hideUpload">
                           <div class="col-sm-9"><input type="file" id="file" name="file"  class="inputfile" onchange="angular.element(this).scope().uploadFile(this.files)"/>
                           </div>
                          </div>                           
                             <div class="row" ng-hide="hideUpload">
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
                            <div class="col-sm-12" ng-hide="hideUpload">
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
                $scope.model = { Id: 0, FileName: '', VideoName: '', GenreType: 1, VideoDescription: '', CatagoryName: 2 };
                $scope.genreData = [];
                $scope.catagoryData = [];
                $scope.uploadFileName = 'Click Browse to select file....';
                $scope.hideUpload = true;

                if (index !== 0) {
                    $scope.hideUpload = true;
                } else {
                    $scope.hideUpload = false;
                }

                $http({
                    url: '/VideoLibrary/GetGenreList',
                    method: 'get'
                }).then(function (response) {
                    //console.log(response)
                    var genreData = response.data.GenreList;
                    for (i = 0; i < genreData.length; i++) {
                        var rowModel = { Id: genreData[i].GenreId, GenreType: genreData[i].GenreType };
                        $scope.genreData.push(rowModel);
                    }
                    $scope.model.GenreType = $scope.genreData[0].Id;
                    var catagoryData = response.data.CatagoryList;
                    for (i = 0; i < catagoryData.length; i++) {
                        var catRowModel = { Id: catagoryData[i].Id, Name: catagoryData[i].Name };
                        $scope.catagoryData.push(catRowModel);
                    }                    
                    $scope.model.CatagoryName = $scope.catagoryData[0].Id;
                });

                if (rowIndex !== 0) {
                    //console.log(index)
                    var data = { id: index };
                    $http({
                        url: '/VideoLibrary/EditData',
                        method: 'post',
                        data: data
                    }).then(function (response) {
                        console.log(response)
                        $scope.model.VideoName = response.data.Model.Name;
                        $scope.model.FileName = response.data.Model.FileName;
                        $scope.model.GenreType = response.data.Model.GenreId;
                        $scope.model.Id = index;

                        // $scope.model.videoDescription = response.data.Model.GenreId;
                    })
                }

                $scope.uploadFile = function (test) {
                    $scope.uploadFileName = test[0].name;
                    // now trigger a view model update
                    $scope.model.FileName = test[0].name;
                    if ($scope.model.VideoName == "") {
                        $scope.model.VideoName = test[0].name.slice(0, -4);
                    }
                    $scope.$apply();
                }

                $scope.Upload = function () {

                    if ($scope.hideUpload !== true) {
                        $scope.disableCancel = true;
                        var files = document.getElementById('file').files;
                        var file = files[0];

                        $scope.fileSize = file.size;

                        var filetype = file.type;
                        var filename = file.name;
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
                                $http({
                                    url: '/VideoLibrary/AddEditVideoLibrary',
                                    method: 'post',
                                    data: $scope.model
                                }).then(function (response) {
                                    console.log(response);
                                    if (response.data.success) {
                                        var saveModel = {
                                            Id: response.data.displayListModel.Id, GenreType: response.data.displayListModel.GenreType,
                                            GenreId: response.data.displayListModel.GenreType, Name: response.data.displayListModel.Name
                                        };
                                        superListAddToModel(saveModel);
                                        modalId.close();
                                    }                                  
                                });
                            }
                        }, 1000);
                    } else {
                        console.log($scope.model);
                        $http({
                            url: '/VideoLibrary/AddEditVideoLibrary',
                            method: 'post',
                            data: $scope.model
                        }).then(function (response) {
                            console.log(response);
                            var genreText = '';
                            for (i = 0; i < $scope.genreData.length; i++) {
                                if ($scope.genreData[i].Id === $scope.model.GenreType) {
                                    genreText = $scope.genreData[i].GenreType;
                                }
                            }
                            $scope.model.GenreType = genreText;

                            var catText = '';
                            for (i = 0; i < $scope.catagoryData.length; i++) {
                                if ($scope.catagoryData[i].Id === $scope.model.Catagory) {
                                    catText = $scope.catagoryData[i].Name;
                                }
                            }
                            $scope.model.CatagoryName = catText;

                            superListAddToModel($scope.model, rowIndex);
                            modalId.close();
                        });
                    }
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
    };

});



