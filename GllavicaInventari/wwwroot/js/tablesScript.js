$(document).ready(function () {

    $('#ProductsTable').DataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2],
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2]
                }
            }
        ]
    });

    $("#SuppliersTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }
        ]
    });

    $("#WarehousesTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }
        ]
    });

    var oTable = $("#EntriesTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            }
        ]
    });

    $("#ExitsTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            }
        ]
    });

    $("#DetailsTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            }
        ]
    });

    $("#TransfersTable").dataTable({
        dom: 'lBfrtip',
        buttons: [
            { extend: 'copy', text: '<i class="fa fa-copy"></i> KOPJO', className: 'copyButton btn-success' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i> PRINTO', className: 'printButton btn-inverse', autoPrint: true,
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).find('table').css('width', '100%');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('text-align', 'center');
                    });
                    $(win.document.body).find('h1').css('text-align', 'center');
                },
                printOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: "csv", text: '<i class="fa fa-file-excel-o"></i> EXCEL', className: "csvButton  btn-success",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }
        ]
    });


    $("#datepicker_from").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        let id = $("#wareHouseDetailsId").val();
        let dateStart = $(this).val();
        let dateEnd = $("#datepicker_to").val()
        window.location.href = "/Stock/Details/" + id + "/" + dateStart + "/" + dateEnd;
    });

    $("#datepicker_to").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        let id = $("#wareHouseDetailsId").val();
        let dateEnd = $(this).val();
        let dateStart = $("#datepicker_from").val();
        window.location.href = "/Stock/Details/" + id + "/" + dateStart + "/" + dateEnd;
    });

    $("#entries_datepicker_from").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        //let id = $("#wareHouseDetailsId").val();
        let dateStart = $(this).val();
        let dateEnd = $("#entries_datepicker_to").val();
        window.location.href = "/Entries/Index/" + dateStart + "/" + dateEnd;
    });

    $("#entries_datepicker_to").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        //let id = $("#wareHouseDetailsId").val();
        let dateEnd = $(this).val();
        let dateStart = $("#entries_datepicker_from").val();
        window.location.href = "/Entries/Index/" + dateStart + "/" + dateEnd;
    });

    $("#exits_datepicker_from").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        let dateStart = $(this).val();
        let dateEnd = $("#exits_datepicker_to").val();
        window.location.href = "/Exits/Index/" + dateStart + "/" + dateEnd;
    });

    $("#exits_datepicker_to").datepicker({ format: 'mm-dd-yyyy' }).on("changeDate", function () {
        let dateEnd = $(this).val();
        let dateStart = $("#exits_datepicker_from").val();
        window.location.href = "/Exits/Index/" + dateStart + "/" + dateEnd;
    });



    $("#entryId").bootstrapValidator();

    $("#supplierCreateForm").bootstrapValidator();
    $("#supplierEditForm").bootstrapValidator();

    $("#warehouseCreateForm").bootstrapValidator();
    $("#warehouseEditForm").bootstrapValidator();



    $("#productsForm").bootstrapValidator();
    $("#exitId").bootstrapValidator();
    $("#transferId").bootstrapValidator();
    $(".select-product").select();
    $("#hastvsh-select").select();
    $("#supplier-select").select();

})