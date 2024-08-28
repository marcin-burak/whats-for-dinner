import { FC, useState } from 'react';
import { Outlet } from 'react-router-dom';
import logo from '../assets/logo.svg';
import styles from './MainLayout.module.scss';
import { Button, Dialog, DialogActions, DialogBody, DialogSurface, DialogTitle, Image, makeStyles, tokens, Tooltip } from '@fluentui/react-components';
import { ClipboardBulletListLtrRegular, PersonCircle32Regular, PowerFilled } from '@fluentui/react-icons';
import { AppItem, Hamburger, NavDrawer, NavDrawerBody, NavDrawerHeader, NavItem } from '@fluentui/react-nav-preview';

export const MainLayout: FC = () => {

    const [showSignOutDialog, setShowSignOutDialog] = useState(false);

    return (
        <div className={styles.layout}>
            <Header onSignOutButtonClicked={() => setShowSignOutDialog(true)} />
            <SideNavigation />
            <PageContent />
            <Dialog open={showSignOutDialog}>
                <DialogSurface>
                    <DialogTitle>
                        Czy na pewno chcesz się wylogować?
                    </DialogTitle>
                    <DialogBody>
                        <DialogActions>
                            <Button appearance="primary">Wyloguj się</Button>
                            <Button onClick={() => setShowSignOutDialog(false)}>Anuluj</Button>
                        </DialogActions>
                    </DialogBody>
                </DialogSurface>
            </Dialog>
        </div>
    );
};

const Header: FC<{ onSignOutButtonClicked: () => void }> = ({ onSignOutButtonClicked }) => (
    <header className={styles.header}>
        <div className={styles.logo}>
            <Image src={logo} width={48} height={48} alt='Logo' />
            <h1>What's for dinner</h1>
        </div>
        <Tooltip content={'Wyloguj się'} relationship={'description'}>
            <Button icon={<PowerFilled />} shape='circular' onClick={onSignOutButtonClicked} />
        </Tooltip>
    </header>
);

const SideNavigation: FC = () => {

    const [isOpen, setIsOpen] = useState(true);

    return (
        <aside>
            <NavDrawer open={isOpen} type='inline' defaultSelectedValue={"1"}>
                <NavDrawerBody>
                    <NavItem href={"/"} icon={<ClipboardBulletListLtrRegular />} value="1">
                        Lista posiłków
                    </NavItem>
                </NavDrawerBody>
            </NavDrawer>
        </aside>
    );
};

const PageContent = () => (
    <article>
        <Outlet />
    </article>
);