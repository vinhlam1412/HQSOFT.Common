$(function () {
    var l = abp.localization.getResource("Common");
	
	var notificationService = window.hQSOFT.common.notifications.notification;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/Notifications/CreateModal",
        scriptUrl: abp.appPath + "Pages/Common/Notifications/createModal.js",
        modalClass: "notificationCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/Notifications/EditModal",
        scriptUrl: abp.appPath + "Pages/Common/Notifications/editModal.js",
        modalClass: "notificationEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            fromUserId: $("#FromUserIdFilter").val(),
			toUserId: $("#ToUserIdFilter").val(),
			notiTitle: $("#NotiTitleFilter").val(),
			notiBody: $("#NotiBodyFilter").val(),
            isRead: (function () {
                var value = $("#IsReadFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
			docId: $("#DocIdFilter").val(),
			url: $("#UrlFilter").val(),
			type: $("#TypeFilter").val()
        };
    };
    
    

    var dataTable = $("#NotificationsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(notificationService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.Notifications.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.Notifications.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    notificationService.delete(data.record.id)
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
			{ data: "toUserId" },
			{ data: "notiTitle" },
			{ data: "notiBody" },
            {
                data: "isRead",
                render: function (isRead) {
                    return isRead ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
			{ data: "docId" },
			{ data: "url" },
            {
                data: "type",
                render: function (type) {
                    if (type === undefined ||
                        type === null) {
                        return "";
                    }

                    var localizationKey = "Enum:NotificationsType." + type;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            }
        ]
    }));
    
    

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewNotificationButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
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
    
    
    
    
});
