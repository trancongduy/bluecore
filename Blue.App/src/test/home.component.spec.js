import { TestBed } from '@angular/core/testing';
import { HomeComponent } from '../pages/home/home-component/home.component';
describe('component: HomeComponent', function () {
    var fixture;
    beforeEach(function () {
        TestBed.configureTestingModule({
            declarations: [HomeComponent]
        });
        fixture = TestBed.createComponent(HomeComponent);
        fixture.detectChanges();
    });
    it('should display Hello World message', function () {
        var debugEl = fixture.debugElement;
        var h1 = debugEl.nativeElement.querySelector('h1');
        expect(h1.textContent).toEqual('Hello World!');
    });
});
//# sourceMappingURL=home.component.spec.js.map