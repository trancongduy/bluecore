import 'jasmine';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { HomeComponent } from '../pages/home/home-component/home.component';

describe('component: HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ HomeComponent ]
    });
    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
  });

  it('should display Hello World message', () => {
    const debugEl = fixture.debugElement;
    const h1 = debugEl.nativeElement.querySelector('h1');
    expect(h1.textContent).toEqual('Hello World!');
  });
});