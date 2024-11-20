
function textAreaAdjust(element) {
    element.style.height = "1px";
    element.style.height = (25+element.scrollHeight)+"px";
}

function setupSearchAndMove(searchInputId, availableListId, selectedListId, addButtonId, removeButtonId) {
    document.getElementById(searchInputId).addEventListener("input", function () {
        const filter = this.value.toLowerCase();
        const availableItems = document.getElementById(availableListId);

        Array.from(availableItems.options).forEach(option => {
            const text = option.text.toLowerCase();
            option.style.display = text.includes(filter) ? "" : "none";
        });
    });
    
    let selectedList = document.getElementById(selectedListId);
    
    for (let f of document.getElementsByTagName("form")) {
        f.addEventListener("submit", () => {
            const options = [...selectedList.options];
            for (let opt of options) {
                opt.selected = true;
            }
        });
        
    }

    document.getElementById(addButtonId).addEventListener("click", function () {
        moveItems(availableListId, selectedListId);
    });

    document.getElementById(removeButtonId).addEventListener("click", function () {
        moveItems(selectedListId, availableListId);
    });
}

function moveItems(sourceId, targetId) {
    const sourceList = document.getElementById(sourceId);
    const targetList = document.getElementById(targetId);

    Array.from(sourceList.selectedOptions).forEach(option => {
        targetList.appendChild(option);
    });
}

function ConfigurePreview(preview, input){
    let englishStatementPreview = document.getElementById(preview);
    let englishStatementInput = document.getElementById(input);
    textAreaAdjust(englishStatementInput);
    function refresh() {
        englishStatementPreview.innerHTML = marked.parse(englishStatementInput.value);
        renderMathInElement(englishStatementPreview, {
            delimiters: katexDelimiters,
            throwOnError: false
        });
    }
    englishStatementInput.addEventListener("input", ()=> refresh());
    waitForKatex();
    function waitForKatex(){
        if(typeof renderMathInElement !== "undefined"){
            refresh();
        }
        else{
            setTimeout(waitForKatex, 250);
        }
    }

}