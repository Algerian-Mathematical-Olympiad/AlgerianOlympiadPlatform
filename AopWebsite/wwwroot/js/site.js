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

function SetContent(preview, value){
    function refresh() {
        preview.innerHTML = marked.parse(value);
        renderMathInElement(preview, {
            delimiters: katexDelimiters,
            throwOnError: false
        });
    }
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