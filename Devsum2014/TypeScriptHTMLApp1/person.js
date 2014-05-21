var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Company;
(function (Company) {
    var Person = (function () {
        function Person(firstname, lastname, age) {
            if (typeof lastname === "undefined") { lastname = "Andersson"; }
            this.firstname = firstname;
            this.lastname = lastname;
            this.age = age;
        }
        Person.prototype.getFullName = function () {
            return this.firstname + " " + this.lastname;
        };
        return Person;
    })();
    Company.Person = Person;

    var Employee = (function (_super) {
        __extends(Employee, _super);
        function Employee(firstname, lastname, salary) {
            _super.call(this, firstname, lastname);
            this.salary = salary;
        }
        return Employee;
    })(Person);
    Company.Employee = Employee;
})(Company || (Company = {}));

var p = new Company.Person("Magnus", "Härlin");
var e = new Company.Employee("M", "H", 10000);

var firstname = p.firstname;
var lastname = p.lastname;
//# sourceMappingURL=person.js.map
