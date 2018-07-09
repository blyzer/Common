import { Component, ViewContainerRef, OnDestroy, HostListener, isDevMode } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastsManager } from 'ng2-toastr';
import { AuthService } from './core/auth/auth.service';
import { Spinkit } from 'ng-http-loader/spinkits';

@Component({
    // tslint:disable-next-line
    selector: 'body',
    templateUrl: './app.component.html',
})
export class AppComponent implements OnDestroy {

    constructor(private _translate: TranslateService,
        private toastr: ToastsManager,
        private vcr: ViewContainerRef,
        private _authService: AuthService) {

        this.toastr.setRootViewContainerRef(vcr);
        _translate.use('es');

       // this.toastr.success('testing');
    }

    ngOnDestroy() {

    }

    // @HostListener('window:beforeunload ', ['$event'])
    // unloadHandler(event) {
    //     if (!isDevMode()) {
    //         this._authService.logout();
    //     }
    // }
}
