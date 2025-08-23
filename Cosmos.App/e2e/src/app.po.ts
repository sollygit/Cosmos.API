import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get(browser.baseUrl) as Promise<any>;
  }

  navigateToAdd() {
    return browser.get(`${browser.baseUrl}/add`) as Promise<any>;
  }

  getBaseUrl() {
    const swagger = element(by.css('mat-icon#settings-icon')).element(by.xpath('..'));
    return swagger.getAttribute('href') as Promise<string>;
  }

}
