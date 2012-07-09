function RunBenchMark() {
    var suite = new Benchmark.Suite;
    var results = '';

    // add tests
    suite.add('RegExp#test', function () {
        /o/.test('Hello World!');
    })
    .add('String#indexOf', function () {
        'Hello World!'.indexOf('o') > -1;
    })
    // add listeners
    .on('cycle', function (event) {
        results += String(event.target) + '\r\n';
    })
    .on('complete', function () {
        results += 'Fastest is ' + this.filter('fastest').pluck('name');
    })
    // run async
    .run({ 'async': false });

    return results;
}