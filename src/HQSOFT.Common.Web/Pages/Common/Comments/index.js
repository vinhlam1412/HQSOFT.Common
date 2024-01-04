$(function () {
    var l = abp.localization.getResource("Common");
	
	var commentService = window.hQSOFT.common.comments.comment;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/Comments/CreateModal",
        scriptUrl: abp.appPath + "Pages/Common/Comments/createModal.js",
        modalClass: "commentCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/Comments/EditModal",
        scriptUrl: abp.appPath + "Pages/Common/Comments/editModal.js",
        modalClass: "commentEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            fromUserId: $("#FromUserIdFilter").val(),
			content: $("#ContentFilter").val(),
			docId: $("#DocIdFilter").val(),
			url: $("#UrlFilter").val()
        };
    };
    
    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>

    var dataTable = $("#CommentsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(commentService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.Comments.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.Comments.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    commentService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "fromUserId" },
			{ data: "content" },
			{ data: "docId" },
			{ data: "url" }
        ]
    }));
    
    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewCommentButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        commentService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/common/comments/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'fromUserId', value: input.fromUserId }, 
                            { name: 'content', value: input.content }, 
                            { name: 'docId', value: input.docId }, 
                            { name: 'url', value: input.url }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();;
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();;
    });
    
    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>
    
    
});
