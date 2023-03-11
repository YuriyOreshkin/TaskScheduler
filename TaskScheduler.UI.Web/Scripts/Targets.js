
var treeview;
var dropdowntree;

function treeevents() {
    var dropdowntreeview = $("#sendto").data("kendoDropDownTree");
    var tree = dropdowntreeview.treeview;
    treeview = tree;
    dropdowntree = dropdowntreeview;
    tree.bind("dataBound", recipients_dataBound);
    tree.bind("check", recipients_check);
}





//Events on DataBound
function onDataBoundSendto(e) {

    this._savedOld = this.value().slice(0);
}

function childsCheck(arr) {

    for (var i = 0; i < arr.length; i++) {
        var dataItem = treeview.dataSource.get(arr[i]);
        if (dataItem) {
            var node = treeview.findByUid(dataItem.uid);
            disableChilds(node);
        }
    }
}



//on DataBound  Tree
function recipients_dataBound(e) {
   
    if (e.node) {
        if (treeview.dataItem(e.node).checked) {
            disableChilds(e.node);
        };
    };
}
//on Check Tree
function recipients_check(e) {

    disableChilds(e.node);
}


//Events on Change
function onChangeSendto(e) {

    var previous = this._savedOld;
    var current = this.value();

    var del = [];
    var add = [];
    if (previous) {
        del = $(previous).not(current).get();
    }
    if (current) {

        add = $(current).not(previous).get();
    }

    this._savedOld = current.slice(0);


    if (del.length > 0) {

        childsCheck(del);
    }

}

//Disable 
function disableNode(node, checked) {
    var checkbox = $(node).find(":checkbox");
    if (treeview.dataItem(node) && checked) {

        treeview.dataItem(node).set("checked", false);
    }
    checkbox.prop("checked", checked);

    checkbox.attr("disabled", checked);
    if (checked) {

        $(node).find(".k-in").addClass("k-state-disabled");

    } else {

        $(node).find(".k-in").removeClass("k-state-disabled");
    }
}

//Disable Childs
function disableChilds(node) {

    var checked = $(node).find(":checkbox").prop("checked");
    var childNodes = $(".k-item", node);
    childNodes.each(function () {

        disableNode(this, checked);
    });

}