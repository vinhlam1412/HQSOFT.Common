import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonComponent } from './components/common.component';
import { CommonRoutingModule } from './common-routing.module';

@NgModule({
  declarations: [CommonComponent],
  imports: [CoreModule, ThemeSharedModule, CommonRoutingModule],
  exports: [CommonComponent],
})
export class CommonModule {
  static forChild(): ModuleWithProviders<CommonModule> {
    return {
      ngModule: CommonModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<CommonModule> {
    return new LazyModuleFactory(CommonModule.forChild());
  }
}
