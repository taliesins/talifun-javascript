function RunCoffeeScriptCompiler(coffeeScript) {
    var javascriptScript = CoffeeScript.compile(coffeeScript);
    return javascriptScript;
}