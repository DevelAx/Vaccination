"use strict";

(function () {
    updateDeleteTargets();

    const $container = $("#inoculations-container");
    const $rowTemplate = $("#inoculation-row-template").remove();

    $("#add-inoculation-btn").click(onAddRow);

    function onAddRow() {
        const rowsCount = $container.find("tr").length;
        const tempHtml = $rowTemplate[0].outerHTML.replace(/\[i\]/g, `${rowsCount}`);
        const $newRow = $(tempHtml);
        $newRow.appendTo($container).show("slow");
    }

    function updateDeleteTargets() {
        $(".my-delete-btn").click(function () {
            const $target = $(this).closest(".delete-btn-target");
            const $isDeleted = $target.find(".is-deleted");
            $isDeleted.val(true);
            $target.hide("slow");
        });
    }
})();