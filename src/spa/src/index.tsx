import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.module.scss';
import { FluentProvider, webLightTheme } from '@fluentui/react-components';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { MainLayout } from './layout';
import { MealsPage } from './pages';

ReactDOM
	.createRoot(document.getElementById('root') as HTMLElement)
	.render(
		<React.StrictMode>
			<FluentProvider theme={webLightTheme}>
				<BrowserRouter>
					<Routes>
						<Route path='/' element={<MainLayout />}>
							<Route path='' element={<MealsPage />} />
						</Route>
					</Routes>
				</BrowserRouter>
			</FluentProvider>
		</React.StrictMode>
	);