import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DragulaService } from 'ng2-dragula';
import { __core_private_testing_placeholder__ } from '@angular/core/testing';

let _trnasferBagId = 0;

@Component({
  selector: 'app-transfer-panel',
  templateUrl: './transfer-panel.component.html',
  styleUrls: ['./transfer-panel.component.css']
})
export class TransferPanelComponent implements OnInit {
  @Input() availableItems: any[] = [];
  @Input() assignedItems: any[] = [];

  asignedSelected: any;
  availableSelected: any;
  transferBagId: string;
  constructor(private dragulaService: DragulaService) {
    this.transferBagId = `transferBag-${_trnasferBagId++}`;
    dragulaService.setOptions(this.transferBagId, {
      revertOnSpill: true,
    });

    dragulaService.drag.subscribe((value) => {
      this.onDrag(value.slice(1));
    });

    dragulaService.drop.subscribe((value) => {
      this.onDrop(value.slice(1));
    });

    _trnasferBagId++;
  }

  ngOnInit() {

  }

  private onDrag(args) {
    this.keepSorted();
  }

  private onDrop(args) {
    this.keepSorted();
  }

  keepSorted() {
    this.assignedItems.sort((a, b) => a.name - b.name);
  }

  selectAvailable(item: any) {
    this.availableSelected = this.availableSelected !== item ? item : null;
  }

  selectAsigned(item: any) {
    this.asignedSelected = this.asignedSelected !== item ? item : null;
  }

  setItem(item: any) {
    if (!item) {
      return;
    }
    this.moveItem(this.availableItems, this.assignedItems, item);
  }

  removeItem(item: any) {
    if (!item) {
      return;
    }
    this.moveItem(this.assignedItems, this.availableItems, item);
  }

  setAll() {
    this.moveAllItems(this.availableItems, this.assignedItems);
  }

  removeAll() {
    this.moveAllItems(this.assignedItems, this.availableItems);
  }

  moveAllItems(source: any[], target: any[]) {
    for (let i = 0; i < source.length; i++) {
      const element = source[i];
      this.moveItem(source, target, element);
      i--;
    }
  }

  moveItem(source: any[], target: any[], item: any, index?: number) {
    index = index || source.indexOf(item);
    source.splice(index, 1);
    target.push(item);

    this.availableSelected = null;
    this.asignedSelected = null;
  }
}
