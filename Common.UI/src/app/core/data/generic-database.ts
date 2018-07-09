import { BehaviorSubject } from 'rxjs/BehaviorSubject';
export class GenericDatabase<T> {

    dataChange: BehaviorSubject<T[]> = new BehaviorSubject<T[]>([]);

    get data(): T[] { return this.dataChange.value; }
    get total(): number { return this.dataChange.value.length; }

    addData(data: T[]) {
        this.dataChange.next(data);
    }
}
