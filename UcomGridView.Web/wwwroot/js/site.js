const tbody = document.getElementById("grid").getElementsByTagName("tbody")[0];
const statuses = ["Active", "Inactive"];
var filter = {
    SearchFilter: "",
    sorting: { columnName: "", orderBy: "" }
};

function getStatusName(statusId) {
    return statusId == 1 ? statuses[0] : statuses[1];
}

function getStatusId(statusName) {
    return statusName == statuses[0] ? 1 : 2;
}

function refresh(take, page) {
    tbody.innerHTML = "";
    $.ajax({
        type: "POST",
        url: "/api/user/get-users",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            take: take,
            page: page,
            filter
        }),
        success: function () {
        },
        error: function () {
            alert("An error has occurred while saving data!");
        },
        async: false,
    }).done(function (result) {
        let take = $("#take").val();
        for (let r = 0; r < take; ++r) {
            let row = tbody.insertRow(r);
            let obj = result[r];
            row.setAttribute("id", obj.id);
            row.insertCell(0).innerText = obj.firstname;
            row.insertCell(1).innerText = obj.lastname;
            row.insertCell(2).innerText = obj.age;
            row.insertCell(3).innerText = obj.email;
            row.insertCell(4).innerText = obj.createdAt;
            row.insertCell(5).innerText = obj.updatedAt;
            row.insertCell(6).innerText = obj.isDeleted;
            row.insertCell(7).innerHTML = '<img class="avatar" src="' + (obj.avatar != null ? obj.avatar : "") + '">';
            row.insertCell(8).innerText = getStatusName(obj.statusId);
            row.insertCell(9).innerHTML = '<button class="fas fa-pencil-alt" id="edit" onClick="edit(this)"></button>' +
                '<button class="fa fa-save" id="save" style="display: none;" onClick="save(this)"></button>' +
                '<button class="fa fa-close" id="close" style="display: none;" onClick="closeEdit(this)"></button>' +
                '<button class="far fa-trash-alt" id="delete" onClick="deleteRow(this)"></button>';
        }
    });
}

$(document).ready(function () {
    refresh($("#take").val(), $("#page").val());
});

function edit(ctx) {
    let childs = ctx.parentElement.parentElement.childNodes;
    let statusId = getStatusId($(childs[8].childNodes[0]).text());

    childs[0].innerHTML = '<input class="form-control" id="firstname" type="text" value="' + childs[0].innerText + '" />';
    childs[1].innerHTML = '<input class="form-control" id="lastname" type="text" value="' + childs[1].innerText + '" />';
    childs[2].innerHTML = '<input class="form-control" id="age" type="text" value="' + childs[2].innerText + '" />';
    childs[3].innerHTML = '<input class="form-control" id="email" type="text" value="' + childs[3].innerText + '" />';
    childs[4].innerHTML = '<input class="form-control" id="createdAt" type="text" value="' + childs[4].innerText + '" />';
    childs[5].innerHTML = '<input class="form-control" id="updatedAt" type="text" value="' + childs[5].innerText + '" />';
    childs[6].innerHTML = '<input id="isDeleted" type="checkbox" ' + (childs[6].checked ? 'checked' : '') + ' />';
    childs[7].innerHTML = '<input class="file-input" id="avatar" type="file" base64attr="' + childs[7].childNodes[0].src + '" />';
    childs[8].innerHTML = '<select id="status">' +
        '<option value="1" ' + (statusId == 1 ? 'selected' : '') + '>Active</option>' +
        '<option value="2" ' + (statusId == 2 ? 'selected' : '') + '>Inactive</option>' +
        '</select>';

    $(ctx).hide();
    $(ctx.parentNode.childNodes[1]).show();
    $(ctx.parentNode.childNodes[2]).show();
}

function save(ctx) {
    let childs = ctx.parentElement.parentElement.childNodes;

    var formData = new FormData();
    formData.append("id", ctx.parentElement.parentElement.getAttribute("id"));
    formData.append("firstname", childs[0].childNodes[0].value);
    formData.append("lastname", childs[1].childNodes[0].value);
    formData.append("age", childs[2].childNodes[0].value);
    formData.append("email", childs[3].childNodes[0].value);
    formData.append("createdAt", childs[4].childNodes[0].value);
    formData.append("updatedAt", childs[5].childNodes[0].value);
    formData.append("isDeleted", childs[6].childNodes[0].checked);
    formData.append("avatar", childs[7].childNodes[0].files[0]);
    formData.append("statusId", childs[8].childNodes[0].value);

    $.ajax({
        url: "/api/user/update",
        type: "PUT",
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function () {
            alert("Saved successfully!");
        },
        error: function () {
            alert("An error has occurred while updating data!");
        },
    }).done(function (avatar) {
        closeEdit(ctx, avatar);
    });
}

function closeEdit(ctx, avatar) {
    let childs = ctx.parentElement.parentElement.childNodes;

    childs[0].innerHTML = childs[0].childNodes[0].value;
    childs[1].innerHTML = childs[1].childNodes[0].value;
    childs[2].innerHTML = childs[2].childNodes[0].value;
    childs[3].innerHTML = childs[3].childNodes[0].value;
    childs[4].innerHTML = childs[4].childNodes[0].value;
    childs[5].innerHTML = childs[5].childNodes[0].value;
    childs[6].innerHTML = childs[6].childNodes[0].checked;
    childs[7].innerHTML = '<img class="avatar" src="' + (avatar != undefined
        ? avatar
        : (childs[7].childNodes[0].getAttribute("base64attr") != null &&
            childs[7].childNodes[0].getAttribute("base64attr").indexOf("data") == 0
            ? childs[7].childNodes[0].getAttribute("base64attr") : "")
        ) + '">';
    childs[8].innerHTML = getStatusName(childs[8].childNodes[0].value);

    $(ctx.parentNode.childNodes[0]).show();
    $(ctx.parentNode.childNodes[1]).hide();
    $(ctx.parentNode.childNodes[2]).hide();
}

function deleteRow(ctx) {
    if (confirm("Are you sure you want to delete this line?") == false)
        return;

    $.ajax({
        url: "/api/user/delete/" + ctx.parentElement.parentElement.getAttribute("id"),
        type: "DELETE",
        success: function () {
            alert("Saved successfully!");
        },
        error: function () {
            alert("An error has occurred while deleting data!");
        },
        async: false,
    }).done(function (result) {
        var i = ctx.parentNode.parentNode.rowIndex;
        document.getElementById("grid").deleteRow(i);
    });
}

$("#insert").click(function () {
    tbody.innerHTML = "";
    $.ajax({
        url: "/api/user/",
        context: document.body,
        success: function () {
            alert("Saved successfully!");
        },
        error: function () {
            alert("An error has occurred while deleting data!");
        },
        async: false,
    }).done(function (result) {
    });
});

$("#prev").click(function () {
    $("#page").val(parseInt($("#page").val()) - 1);
    refresh(parseInt($("#take").val()), $("#page").val());
});

$("#next").click(function () {
    $("#page").val(parseInt($("#page").val()) + 1);
    refresh($("#take").val(), $("#page").val());
});

$("#page").change(function () {
    refresh($("#take").val(), $("#page").val());
});

$("#take").change(function () {
    refresh($("#take").val(), $("#page").val());
});

$("#sort-firstname").click(function () {
    revertOrder("Firstname");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-lastname").click(function () {
    revertOrder("Lastname");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-age").click(function () {
    revertOrder("Age");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-email").click(function () {
    revertOrder("Email");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-isDeleted").change(function () {
    revertOrder("IsDeleted");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-createdAt").click(function () {
    revertOrder("CreatedAt");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-updatedAt").click(function () {
    revertOrder("UpdatedAt");
    refresh($("#take").val(), $("#page").val());
});

$("#sort-status").change(function () {
    revertOrder("Status");
    refresh($("#take").val(), $("#page").val());
});

function setFilterSearchText(searchText) {
    filter.SearchFilter = searchText;
}

function revertOrder(columnName) {
    filter.sorting.ColumnName = columnName;
    filter.sorting.orderBy = filter.sorting.orderBy == "ASC" ? "DESC" : "ASC";
}