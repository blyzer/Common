import { TransferPanelModule } from './../core/transfer-panel/transfer-panel.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { SettingsRoutes } from './settings.routing';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { DataTableModule, SharedModule, CheckboxModule } from 'primeng/primeng';
import { TabsModule, CollapseModule, ModalModule, BsModalService } from 'ngx-bootstrap';
import { SchoolListComponent } from './schools/school-list/school-list.component';
import { SchoolEditComponent } from './schools/school-edit/school-edit.component';

import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import {TableModule} from 'primeng/table';

import { RoleListComponent } from './roles/role-list/role-list.component';
import { RoleEditComponent } from './roles/role-edit/role-edit.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SettingsRoutes,
        TranslateModule.forChild(),
        DataTableModule,
        SharedModule,
        TabsModule,
        TransferPanelModule.forRoot(),
        CheckboxModule,
        CollapseModule,
        ModalModule.forRoot(),
        MultiselectDropdownModule,
        TableModule
    ],
    declarations: [
        UserListComponent,
        UserEditComponent,
        SchoolListComponent,
        SchoolEditComponent,
        RoleListComponent,
        RoleEditComponent
    ],
    providers: [
        BsModalService
    ]
})
export class SettingsModule { }
