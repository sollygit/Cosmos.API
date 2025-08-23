import { AppPage } from './app.po';
import { browser, logging } from 'protractor';
import { environment } from '../../src/environments/environment';

describe('CosmosApp', () => {
  let page: AppPage;

  beforeAll(async () => {
    page = new AppPage();
  });

  it('should display page title', async () => {
    await page.navigateTo();
    expect(await browser.getTitle()).toEqual('CosmosApp');
  });

  it('should have a valid base URL', async () => {
    await page.navigateTo();
    const baseUrl = await page.getBaseUrl();
    expect(baseUrl).toEqual(`${environment.baseUrl}/`);
  });

  it('should navigate to the Add Movie', async () => {
    await page.navigateToAdd();
    expect(await browser.getCurrentUrl()).toContain('/add');
  });

  afterEach(async () => {
    // Assert that there are no errors emitted from the browser
    const logs = await browser.manage().logs().get(logging.Type.BROWSER);
    expect(logs).not.toContain(jasmine.objectContaining({
      level: logging.Level.SEVERE,
    } as logging.Entry));
  });
});
