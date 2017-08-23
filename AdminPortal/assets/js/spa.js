
    $(function () {
        generateYearDropDown();
    loadTable();
        getEmployeeList();

        var min = 2015,
            max = new Date().getFullYear(),
            select = document.getElementById('newEntryYearList');

        for (var i = min; i <= max; i++) {
            var opt = document.createElement('option');
            opt.value = i;
            opt.innerHTML = i;
            select.appendChild(opt);
        };

        $('#YearStartDate').datetimepicker({format: 'DD/MM/YYYY' });
        $('#YearEndDate').datetimepicker({format: 'DD/MM/YYYY' });
        $('#OtherSalaryStartDate').datetimepicker({format: 'DD/MM/YYYY' });
        $('#OtherSalaryEndDate').datetimepicker({format: 'DD/MM/YYYY' });

        $("#YearStartDate").on("dp.change", function (e) {
            if ($('#YearEndDate').val() < $('#YearStartDate').val()) {
        $("#YearEndDate").val($("#YearStartDate").val())
    }

    });

        $("#YearEndDate").on("dp.change", function (e) {
            if ($('#YearStartDate').val() > $('#YearEndDate').val()) {
        $("#YearStartDate").val($("#YearEndDate").val())
    }

    });

        $("#OtherSalaryStartDate").on("dp.change", function (e) {
            if ($('#OtherSalaryEndDate').val() < $('#OtherSalaryStartDate').val()) {
        $("#OtherSalaryEndDate").val($("#OtherSalaryStartDate").val())
    }

    });

        $("#OtherSalaryEndDate").on("dp.change", function (e) {
            if ($('#OtherSalaryStartDate').val() > $('#OtherSalaryEndDate').val()) {
        $("#OtherSalaryStartDate").val($("#OtherSalaryEndDate").val())
    }

    });

        $("#AddOtherSalary").click(function () {
            if ($(this).is(':checked')) {
        $("#showOtherSalary").show();
    }
            else {
        $("#showOtherSalary").hide();
    }
        });

        $('#selectYearList').change(function () {
        loadTable($(this).val());
    });

        $("#btnedit").click(function () {
        console.log("edit button submit clicked");
    $.ajax({
        type: "PUT",
                url: "/webapi/yearly-wage-exp/put-entry-details-for-edit",
                contentType: "application/json",
                data: JSON.stringify({"YearId": $('#YearId').val(), "EmployeeName": $('#EmployeeName').val(), "YearTotal": $('#YearTotal').val() }),
                success: function () {
        $('#editEntryModal').modal('hide');
    loadTable();
                },
                error: function (data, statusCode) {
        console.log("unable to perform action, error: " + statusCode + "\ndata" + data);
    }

            });

        });

        $("#confirmDeleteBtn").click(function () {
        $.ajax({
            url: "/webapi/yearly-wage-exp/delete/" + $('#YearIdToDelete').val(),
            type: "DELETE",
            contentType: "application/json",
            success: function (data) {
                $("#deleteEntryModal").modal("hide");
                loadPage();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log("Error, couldn't delete(xhr,errorThrown,textStatus) " + xhr + "," + errorThrown + "," + textStatus);
            }
        });


    });

        $('#DateJoinedForQuickAdd').datetimepicker({format: 'DD/MM/YYYY' });
        $('#ContractEndDateForQuickAdd').datetimepicker({format: 'DD/MM/YYYY' });
        $('#DateOfBirthForQuickAdd').datetimepicker({format: 'DD/MM/YYYY' });

        $("#DateJoinedForQuickAdd").on("dp.change", function (e) {
            if ($('#ContractEndDateForQuickAdd').val() < $('#DateJoinedForQuickAdd').val()) {
        $("#ContractEndDateForQuickAdd").val($("#DateJoinedForQuickAdd").val())
    }
    });

        $("#quickAddNewEmployeeBtn").click(function () {
            var passData = {
        "Name": $('#EmployeeNameForQuickAdd').val(),
                "Department": $('#DepartmentForQuickAdd').val(),
                "DateJoined": moment($('#DateJoinedForQuickAdd').val(), "MM-DD-YYYY"),
                "ContractEndDate": moment($('#ContractEndDateForQuickAdd').val(), "MM-DD-YYYY"),
                "DateOfBirth": moment($('#DateOfBirthForQuickAdd').val(), "MM-DD-YYYY"),
                "CurrentSalary": $('#CurrentSalaryForQuickAdd').val()
            };
            $.ajax({
        url: "/webapi/yearly-wage-exp/quick-add-new-employee/",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(passData),
                success: function (data) {
        $("#quickAddNewEmployee").modal("hide");
    loadPage();
                },
                error: function (xhr, textStatus, errorThrown) {
        console.log("Error, couldn't delete(xhr,errorThrown,textStatus) " + xhr + "," + errorThrown + "," + textStatus);
    }
            });
        });


        $("#quickAddNewEntrySaveBtn").click(function () {
            var passData = {
        "EmployeeId": $('#EmployeeDropdown').val(),
                "BusinessYear": $('#newEntryYearList').val(),
                "CurrentSalaryStartDate": moment($('#YearStartDate').val(), "MM-DD-YYYY"),
                "CurrentSalaryEndDate": moment($('#YearEndDate').val(), "MM-DD-YYYY"),
                "OtherSalaryStartDate": moment($('#OtherSalaryStartDate').val(), "MM-DD-YYYY"),
                "OtherSalaryEndDate": moment($('#OtherSalaryEndDate').val(), "MM-DD-YYYY"),
                "OtherSalary": $('#OtherSalary').val(),
                "SalesCommissionBonus": $('#SalesCommissionBonus').val(),
                "LoyaltyBonus": $('#LoyaltyBonus').val(),
                "ReferalBonus": $('#ReferalBonus').val(),
                "OtherBonus": $('#OtherBonus').val(),
                "HolidayBonus": $('#HolidayBonus').val(),
                "MissionBonus": $('#MissionBonus').val()
            };
            $.ajax({
        url: "/webapi/yearly-wage-exp/quick-add-new-entry/" + $("#newEntryYearList").val(),
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(passData),
                success: function (data) {
        $("#quickAddNewEntry").modal("hide");
    loadPage();
                },
                error: function (xhr, textStatus, errorThrown) {
        console.log("Error, couldn't delete(xhr,errorThrown,textStatus) " + xhr + "," + errorThrown + "," + textStatus);
    }
            });
        });

    });

    var generateYearDropDown = function () {
        var min = 2015,
            max = new Date().getFullYear(),
            select = document.getElementById('selectYearList');

        for (var i = min; i <= max; i++) {
            var opt = document.createElement('option');
            opt.value = i;
            opt.innerHTML = i;
            select.appendChild(opt);
        }
    };

    var loadTable = function (businessYear) {
        $.getJSON("/webapi/yearly-wage-exp/get-table-data/" + businessYear)
            .done(function (data) {
                var table = $('<table/>').addClass("table table-striped");
                var headerFields = $('<thead/>')
                    .append($("<th/>").text("Year Id"))
                    .append($("<th/>").text("Business Year"))
                    .append($("<th/>").text("Employee Id"))
                    .append($("<th/>").text("Employee Name"))
                    .append($("<th/>").text("Year Total"))
                    .append($("<th/>").text());
                table.append(headerFields);
                table.append($("<tbody>"));
                $.each(data, function (i, val) {
                    var row = $("<tr/>");
                    row.append($("<td/>").html(val.YearId));
                    row.append($("<td/>").html(val.BusinessYear));
                    row.append($("<td/>").html(val.EmployeeId));
                    row.append($("<td/>").html(val.EmployeeName));
                    row.append($("<td/>").html(val.YearTotal));
                    var editEntry = $('<input/>').addClass("btn btn-sm btn-warning").val('Edit Entry').prop("type", "submit").attr("data-toggle", "modal").attr("data-target", "#editEntryModal").click(function () {
                        $.ajax({
                            cache: false,
                            url: "/webapi/yearly-wage-exp/get-entry-details-for-edit/" + val.YearId,
                            contentType: "application/json",
                            type: "GET",
                            success: function (data) {
                                console.log(data.YearId);
                                $("#YearId").val(data.YearId);
                                $("#EmployeeName").val(data.EmployeeName);
                                $("#YearTotal").val(data.YearTotal);
                            },
                        });
                    });
                    var deleteEntry = $('<input/>').addClass("btn btn-sm btn-danger").val('Delete Entry').prop("type", "submit").attr("data-toggle", "modal").attr("data-target", "#deleteEntryModal").click(function () {
                        $("#YearIdToDelete").val(val.YearId)
                        $("#employeeNameToDelete").append(val.EmployeeName);
                        $("#businessYearFromToDelete").append(val.BusinessYear);
                    });
                    row.append($("<td/>").append($("<div/>").addClass("btn-group").append(editEntry).append(deleteEntry)));
                    table.append(row);
                });
                table.append($("</tbody>"));
                $('#generateTable').html(table);

            });

    };

    var getEmployeeList = function () {
        $.ajax({
            type: "get",
            url: "/webapi/yearly-wage-exp/get-employee-list-for-new-entry",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: {},
            success: function (result) {
                $.each(result, function (i) {
                    $('#EmployeeDropdown').append($('<option></option>').val(result[i].Value).html(result[i].Text));
                });
            },
            failure: function () {
                alert("Error");
            }
        });
    }
