module Company {
    export class Person {
        constructor(public firstname: string, public lastname: string = "Andersson", private age?: number) {
        }
    }

    export class Employee extends Person {
        constructor(firstname: string, lastname: string, public salary: number) {
            super(firstname, lastname);
        }
    }
}

var p = new Company.Person("Magnus", "Härlin");
var e = new Company.Employee("M", "H", 10000);

var firstname = p.firstname;
var lastname = p.lastname;
