import { TransferPanelComponent } from './transfe-panel/transfer-panel.component';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragulaModule } from 'ng2-dragula';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
    imports: [
        CommonModule,
        DragulaModule,
        TranslateModule.forChild(),
    ],
    declarations: [TransferPanelComponent],
    exports: [
        TransferPanelComponent
    ]
})
export class TransferPanelModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: TransferPanelModule,
        };
    }
}
