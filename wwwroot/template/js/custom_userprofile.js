if ($('#p-fam').length > 0) {
    function removeSib(idFam) {
        $.ajax({
            url: "/User/API/RemoveFam",
            type: "post",
            data: {
                id: idFam
            },
            success: function (response) {
                window.location.reload();
            }, 
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    }
}
function removeSW(id, table) {
    $.ajax({
        url: "/User/API/RemoveSW",
        type: "post",
        data: {
            id: id,
            table: table
        },
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function 