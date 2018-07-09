import { Component } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import 'rxjs/add/operator/filter';

@Component({
  selector: 'app-breadcrumbs',
  template: `
  <ng-template ngFor let-breadcrumb [ngForOf]="breadcrumbs" let-last = last>
    <li class="breadcrumb-item"
        *ngIf="breadcrumb.label.title&&breadcrumb.url.substring(breadcrumb.url.length-1) == '/'||breadcrumb.label.title&&last"
        [ngClass]="{active: last}">
      <a *ngIf="!last" [routerLink]="breadcrumb.url">{{breadcrumb.label.title | translate }}</a>
      <span *ngIf="last" [routerLink]="breadcrumb.url">{{breadcrumb.label.title | translate }}</span>
    </li>
  </ng-template>`
})
export class BreadcrumbsComponent {
  breadcrumbs: Array<Object>;
  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.router.events.filter(event => event instanceof NavigationEnd).subscribe((event) => {
      this.breadcrumbs = [];
      let currentRoute = this.route.root,
        url = '';
      do {
        const childrenRoutes = currentRoute.children;
        currentRoute = null;
        childrenRoutes.forEach(childRoute => {
          if (childRoute.outlet === 'primary') {
            const routeSnapshot = childRoute.snapshot;
            url += '/' + routeSnapshot.url.map(segment => segment.path).join('/');

            const data = childRoute.snapshot.data;
            const exists = this.breadcrumbs.some((b: any) => b.label.title === data.title);
            if (!exists && !!data.title) {
              this.breadcrumbs.push({
                label: data,
                url: url
              });
            }
            currentRoute = childRoute;
          }
        });
      } while (currentRoute);
    });
  }
}
