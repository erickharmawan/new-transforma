import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { provideEchartsCore } from 'ngx-echarts';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import * as echarts from 'echarts/core';
import { BarChart, LineChart, PieChart } from 'echarts/charts';
import { GridComponent, TooltipComponent, TitleComponent, LegendComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { authInterceptor } from './core/interceptors/auth.interceptor';

// Register ECharts components
echarts.use([BarChart, LineChart, PieChart, GridComponent, TooltipComponent, TitleComponent, LegendComponent, CanvasRenderer]);

registerLocaleData(en);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), 
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync(),
    provideNzI18n(en_US),
    provideEchartsCore({ echarts })
  ]
};
