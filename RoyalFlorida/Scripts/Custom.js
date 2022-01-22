$(function () {
    var det = new Date();
    var day = parseInt(det.getDate()) + 1
    var month = parseInt(det.getMonth())
    var year = parseInt(det.getFullYear())
    $('#RequestedDate').datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: new Date(year, month, day),
        showOn: "both",
        buttonText: "<i class='fa fa-calendar'></i>"
    });
});