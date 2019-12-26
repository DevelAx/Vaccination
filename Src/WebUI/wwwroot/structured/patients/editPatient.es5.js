"use strict";

(function () {
    updateDeleteTargets();

    var $container = $("#inoculations-container");
    var $rowTemplate = $("#inoculation-row-template").remove();

    $("#add-inoculation-btn").click(onAddRow);

    function onAddRow() {
        var rowsCount = $container.find("tr").length;
        var tempHtml = $rowTemplate[0].outerHTML.replace(/\[i\]/g, "" + rowsCount);
        var $newRow = $(tempHtml);
        $newRow.appendTo($container).show("slow");
    }

    function updateDeleteTargets() {
        $(".my-delete-btn").click(function () {
            var $target = $(this).closest(".delete-btn-target");
            var $isDeleted = $target.find(".is-deleted");
            $isDeleted.val(true);
            $target.hide("slow");
        });
    }
})();

