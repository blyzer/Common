// import { BehaviorSubject } from 'rxjs/BehaviorSubject';
// import { GenericDatabase } from './generic-database';
// import { AuthorizedHttp } from './../http/authorized-http.service';
// import { MdPaginator, MdSort } from '@angular/material';
// import { Observable } from 'rxjs/Rx';
// import { CollectionViewer } from '@angular/cdk/collections';
// import { DataSource } from '@angular/cdk/collections';

// export class GenericDatasoruce<T> extends DataSource<T>  {
//     private _filterChange = new BehaviorSubject('');

//     public get filter(): string { return this._filterChange.value; }

//     constructor(private _dataBase: GenericDatabase<T>,
//         private _paginator: MdPaginator,
//         private _sort: MdSort) {

//         super();
//     }

//     connect(collectionViewer: CollectionViewer): Observable<T[]> {
//         const displayDataChanges = [
//             this._paginator.page,
//             this._sort.mdSortChange,
//             this._dataBase.dataChange
//         ];
//         return Observable.merge(...displayDataChanges).map(() => {
//             return this.getSortedData();
//         });
//     }

//     getSortedData(): T[] {
//         if (!this._sort || !this._sort.active || this._sort.direction === '') {
//             return this._dataBase.data;
//         }

//         return this._dataBase.data.sort((a, b) => {
//             let propertyA: number | string = '';
//             let propertyB: number | string = '';

//             [propertyA, propertyB] = [a[this._sort.active], b[this._sort.active]];

//             const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
//             const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

//             return (valueA < valueB ? -1 : 1) * (this._sort.direction === 'asc' ? 1 : -1);
//         });
//     }

//     disconnect(collectionViewer: CollectionViewer): void {
//         // do nothing
//     }
// }
